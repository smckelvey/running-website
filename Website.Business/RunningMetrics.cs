using System;
using System.Collections.Generic;
using System.Linq;
using Website.Models;
using Website.Common.Extensions;
using Website.Data;

namespace Website.Business
{
    public class RunningMetrics
    {
        private static IWorkoutEventRepository Repository { get; set; }

        private static List<WorkoutEvent> _allRuns;

        public RunningMetrics()
        {
            Repository = new RunningRepository();
        }

        private IEnumerable<WorkoutEvent> AllRuns
        {
            get
            {
                if (_allRuns == null)
                {
                    _allRuns = Repository.GetActivitesByType("Running").ToList();
                    _allRuns.AddRange(Repository.GetActivitesByType("Running (Treadmill)"));
                }
                return _allRuns;
            }
        }

        public int TotalYears => DateTime.Now.Year - AllRuns.First().Date.Year;

        public int TotalRuns => AllRuns.Count();

        public int TotalMiles
        {
            get
            {
                return (int)AllRuns.Sum(x => x.Distance.ToMiles(x.Unit.Name));
            }
        }
        public int TotalHours
        {
            get
            {
                return AllRuns.Sum(x => x.Duration.ToMinutes()) / 60;
            }
        }

        public IEnumerable<WorkoutEvent> GetPastRuns()
        {
            return AllRuns.Where(r => r.Date.Date <= DateTime.Now.Date);
        }
        public WorkoutEvent GetNextRace()
        {
            return AllRuns.Where(r => r.Date > DateTime.Now).OrderBy(r => r.Date).FirstOrDefault();
        }
        public WorkoutEvent GetNextMarathon()
        {
            return AllRuns.Where(r => r.Date > DateTime.Now && r.Distance.ToMiles(r.Unit.Name) > 26).OrderBy(r => r.Date).FirstOrDefault();
        }
    }
}
