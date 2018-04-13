using System;
using System.Collections.Generic;
using System.Linq;
using Website.Models;
using Website.Data;

namespace Website.Business
{
    public class MonthlyData
    {
        private static IWorkoutEventRepository _repo;
        private static List<WorkoutEvent> _allActivities;
        private static string _userName;
        private static DateTime _startDate;

        public MonthlyData(string userName, DateTime startDate)
        {
            _repo = new RunningRepository();
            _userName = userName;
            _startDate = startDate;
        }

        private static IWorkoutEventRepository repo => _repo;

        public IEnumerable<WorkoutEvent> Activities
        {
            get
            {
                //TODO: figure out caching later
                //if (_allActivities == null)
                //{
                    _allActivities = repo.GetActivitiesByDate(_startDate, _startDate.AddMonths(1).AddDays(-1)).Where(a => a.User.Name.ToLower() == _userName.ToLower()).ToList();
                //}
                return _allActivities;
            }
        }
        
    }
}
