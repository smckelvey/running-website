using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using Website.Business;
using Website.Common.Extensions;

namespace Website.Models
{
    public class ProfileViewModel
    {
        public string ProfileName = "";

        public string ProfileImage => ProfileName + "_profile.jpg";

        public string Goal5kTime = "18:45";
        public string Goal10kTime = "39:00";
        public string GoalHalfTime = "1:26:00";
        public string GoalMarathonTime = "3:00:00";
        public string Starting5kTime = "21:00";
        public string Starting10kTime = "45:00";
        public string StartingHalfTime = "1:40:00";
        public string StartingMarathonTime = "3:25:00";


        public int FastestMarathonGoalPercent { get; set; }
        public int FastestHalfGoalPercent { get; set; }
        public int Fastest10kGoalPercent { get; set; }
        public int Fastest5kGoalPercent { get; set; }

        public string Fastest10kDate { get; set; }
        public string Fastest10kRace { get; set; }
        public string Fastest10kTime { get; set; }
        public string Fastest5kDate { get; set; }
        public string Fastest5kRace { get; set; }
        public string Fastest5kTime { get; set; }
        public string FastestHalfDate { get; set; }
        public string FastestHalfRace { get; set; }
        public string FastestHalfTime { get; set; }
        public string FastestMarathonDate { get; set; }
        public string FastestMarathonRace { get; set; }
        public string FastestMarathonTime { get; set; }

        public List<PaceEvent> BestPaces { get; set; }
        public List<TimelineEvent> RecentRaces { get; set; }
        public List<TrainingSeason> DistanceTotals { get; set; }

        public void PopulateTimeline(IndividualMetrics metrics)
        {
            //TODO: pull these out somewhere
            //Timeline views
            var races = metrics.Activities.Where(e => !String.IsNullOrEmpty(e.Title) && e.Date < DateTime.Now && !String.IsNullOrEmpty(e.Duration)).OrderByDescending(e => e.Date).Take(10);
            RecentRaces = new List<TimelineEvent>();
            foreach (var race in races)
            {
                var myEvent = new TimelineEvent();
                var speed = Math.Round((race.Duration.ToMinutes() / race.Distance.ToMiles(race.Unit.Name)), 2);
                myEvent.Pace = speed.ToDuration();
                myEvent.Time = race.Duration;
                myEvent.Distance = race.Distance.ToMiles(race.Unit.Name).ToFriendlyDistance();
                myEvent.Race = race.Title;
                myEvent.Notes = race.Notes;

                RecentRaces.Add(myEvent);
            }
        }

        public void PopulateBestPaces(IndividualMetrics metrics)
        {
            //Best Paces calculations
            BestPaces = new List<PaceEvent>();
            double[] importantDistances = { 3.1, 5, 6.2, 8, 9.3, 10, 12.4, 13.1, 15.5, 20, 26.2, 31.1, 50, 100 };
            foreach (var distance in importantDistances)
            {
                var myEvent = new PaceEvent {Distance = distance.ToString(CultureInfo.InvariantCulture)};

                var bestRun = metrics.Runs.Where(r => r.Distance.ToMiles(r.Unit.Name) >= (decimal)distance).OrderBy(r => r.Duration.ToMinutes() / r.Distance.ToMiles(r.Unit.Name)).FirstOrDefault();
                if (bestRun != null)
                {
                    var speed = Math.Round((bestRun.Duration.ToMinutes() / bestRun.Distance.ToMiles(bestRun.Unit.Name)), 2);
                    myEvent.Pace = speed.ToDuration();
                    myEvent.Time = (distance * (double)speed).ToDuration();
                    BestPaces.Add(myEvent);
                }
            }
        }

        public void PopulateTotalDistances(IndividualMetrics metrics)
        {
            //Total Distances by Season
            var startDate = new DateTime(2010, 1, 1);
            DistanceTotals = new List<TrainingSeason>();
            while (startDate < DateTime.Now)
            {
                var seasonMiles = metrics.Runs.Where(r => r.Date >= startDate && r.Date < startDate.AddMonths(6)).Select(r => r.Distance.ToMiles(r.Unit.Name)).ToList().Sum();
                seasonMiles += metrics.TreadmillRuns.Where(r => r.Date >= startDate && r.Date < startDate.AddMonths(6)).Select(r => r.Distance.ToMiles(r.Unit.Name)).ToList().Sum();
                if (seasonMiles > 0)
                {
                    var trainingSeason =
                        new TrainingSeason {Miles = seasonMiles.ToString(CultureInfo.InvariantCulture)};

                    var season = startDate.Month < 6 ? "Spring" : "Fall";
                    trainingSeason.Season = season + " " + startDate.Year;

                    DistanceTotals.Add(trainingSeason);
                }
                startDate = startDate.AddMonths(6);
            }
            DistanceTotals.Reverse();
        }

        public void PopulateFastestRuns(IndividualMetrics metrics)
        {
            //5k Calculations
            var fastest5k = metrics.FastestRun(RaceDistance.Miles5k);
            Fastest5kRace = fastest5k.Title;
            Fastest5kDate = fastest5k.Date.ToShortDateString();
            Fastest5kTime = fastest5k.Duration;

            //10k Calculations
            var fastest10k = metrics.FastestRun(RaceDistance.Miles10k);
            Fastest10kRace = fastest10k.Title;
            Fastest10kDate = fastest10k.Date.ToShortDateString();
            Fastest10kTime = fastest10k.Duration;

            //Half Calculations
            var fastestHalf = metrics.FastestRun(RaceDistance.MilesHalfMarathon);
            FastestHalfRace = fastestHalf.Title;
            FastestHalfDate = fastestHalf.Date.ToShortDateString();
            FastestHalfTime = fastestHalf.Duration;

            //Marathon Calculations
            var fastestMarathon = metrics.FastestRun(RaceDistance.MilesMarathon);
            FastestMarathonRace = fastestMarathon.Title;
            FastestMarathonDate = fastestMarathon.Date.ToShortDateString();
            FastestMarathonTime = fastestMarathon.Duration;



            //Goals
            decimal Total5kMinutesDifference = Starting5kTime.ToMinutes() - Goal5kTime.ToMinutes();
            decimal Current5kMinutesDifference = fastest5k.Duration.ToMinutes() - Goal5kTime.ToMinutes();
            Current5kMinutesDifference = Current5kMinutesDifference < 0 ? 0 : Current5kMinutesDifference;
            Fastest5kGoalPercent = Convert.ToInt32((1 - (Current5kMinutesDifference / Total5kMinutesDifference)) * 100);

            decimal Total10kMinutesDifference = Starting10kTime.ToMinutes() - Goal10kTime.ToMinutes();
            decimal Current10kMinutesDifference = fastest10k.Duration.ToMinutes() - Goal10kTime.ToMinutes();
            Current10kMinutesDifference = Current10kMinutesDifference < 0 ? 0 : Current10kMinutesDifference;
            Fastest10kGoalPercent = Convert.ToInt32((1 - (Current10kMinutesDifference / Total10kMinutesDifference)) * 100);

            decimal TotalHalfMinutesDifference = StartingHalfTime.ToMinutes() - GoalHalfTime.ToMinutes();
            decimal CurrentHalfMinutesDifference = fastestHalf.Duration.ToMinutes() - GoalHalfTime.ToMinutes();
            CurrentHalfMinutesDifference = CurrentHalfMinutesDifference < 0 ? 0 : CurrentHalfMinutesDifference;
            FastestHalfGoalPercent = Convert.ToInt32((1 - (CurrentHalfMinutesDifference / TotalHalfMinutesDifference)) * 100);

            decimal TotalMarathonMinutesDifference = StartingMarathonTime.ToMinutes() - GoalMarathonTime.ToMinutes();
            decimal CurrentMarathonMinutesDifference = fastestMarathon.Duration.ToMinutes() - GoalMarathonTime.ToMinutes();
            CurrentMarathonMinutesDifference = CurrentMarathonMinutesDifference < 0 ? 0 : CurrentMarathonMinutesDifference;
            FastestMarathonGoalPercent = Convert.ToInt32((1 - (CurrentMarathonMinutesDifference / TotalMarathonMinutesDifference)) * 100);

        }
    }
}