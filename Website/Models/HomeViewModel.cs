using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Website.Business;

namespace Website.Models
{
    public class HomeViewModel
    {
        public int TotalYears { get; set; }
        public int TotalRuns { get; set; }
        public int TotalHours { get; set; }
        public int TotalMiles { get; set; }

        public string NextRace { get; set; }
        public string NextRaceDate { get; set; }
        public string NextRaceLocation { get; set; }

        public string NextMarathon { get; set; }
        public string NextMarathonDate { get; set; }
        public string NextMarathonLocation { get; set; }

        public void PopulateTotals(int years, int runs, int miles, int hours)
        {
            TotalYears = years;
            TotalRuns = runs;
            TotalMiles = miles;
            TotalHours = hours;
        }

        public void PopulateNextMarathon(WorkoutEvent nextMarathon)
        {
            NextMarathon = nextMarathon.Title;
            NextMarathonDate = nextMarathon.Date.ToShortDateString();
            NextMarathonLocation = nextMarathon.Notes;
        }

        public void PopulateNextRace(WorkoutEvent nextRace)
        {
            NextRace = nextRace.Title;
            NextRaceDate = nextRace.Date.ToShortDateString();
            NextRaceLocation = nextRace.Notes;
        }
    }
}
