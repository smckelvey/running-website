using System;
using System.Collections.Generic;
using System.Linq;
using Website.Data;
using Website.Common.Extensions;
using Website.Models;

namespace Website.Business
{
    public class IndividualMetrics
    {
        private static IWorkoutEventRepository _repo;
        private static List<WorkoutEvent> _allActivities;
        private static string _userName;

        public IndividualMetrics(string userName)
        {
            _repo = new RunningRepository();
            _userName = userName;
        }

        private static IWorkoutEventRepository repo => _repo;

        public IEnumerable<WorkoutEvent> Activities
        {
            get
            {
                //TODO: figure out caching later
                //if (_allActivities == null)
                //{
                    _allActivities = repo.GetActivitiesByUser(_userName).ToList();
                //}
                return _allActivities;
            }
        }

        public IEnumerable<WorkoutEvent> Runs
        {
            get
            {
                return Activities.Where(e => e.Date.Date <= DateTime.Now.Date && e.Type.Name.Equals("Running"));
            }
        }

        public IEnumerable<WorkoutEvent> TreadmillRuns
        {
            get
            {
                return Activities.Where(e => e.Date.Date <= DateTime.Now.Date && e.Type.Name.Equals("Running (Treadmill)"));
            }
        }

        public IEnumerable<WorkoutEvent> Rides
        {
            get
            {
                return Activities.Where(e => e.Date.Date <= DateTime.Now.Date && e.Type.Name.Equals("Cycling"));
            }
        }

        public IEnumerable<WorkoutEvent> Swims
        {
            get
            {
                return Activities.Where(e => e.Date.Date <= DateTime.Now.Date && e.Type.Name.Equals("Swimming"));
            }
        }

        public IEnumerable<WorkoutEvent> Upcoming
        {
            get
            {
                return Activities.Where(e => e.Date.Date >= DateTime.Now.Date);
            }
        }

        public int TotalYears => DateTime.Now.Year - Activities.First().Date.Year;

        public WorkoutEvent FastestRun(decimal minimumDistance)
        {
            return Runs.Where(r => r.Distance.ToMiles(r.Unit.Name) >= minimumDistance).OrderBy(r => r.Duration.ToMinutes()).FirstOrDefault();
        }

    }
}
