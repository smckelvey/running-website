using System;
using System.Collections.Generic;
using Website.Models;

namespace Website.Data
{
    public interface IWorkoutEventRepository
    {
        IEnumerable<WorkoutEvent> GetActivitesByType(string activity);
        IEnumerable<WorkoutEvent> GetActivitiesByUser(int userId);
        IEnumerable<WorkoutEvent> GetActivitiesByUser(string name);
        IEnumerable<WorkoutEvent> GetActivitiesByDate(DateTime startDate, DateTime endDate);

        IEnumerable<UnitOfMeasurement> GetUnitOfMeasurements();
        IEnumerable<ActivityType> GetActivityTypes();

        int CreateActivity(WorkoutEvent activity);
        WorkoutEvent ReadActivity(int id);
        void UpdateActivity(WorkoutEvent activity);
        void DeleteActivity(int id);
        IEnumerable<WorkoutEvent> GetAllActivities();
    }
}