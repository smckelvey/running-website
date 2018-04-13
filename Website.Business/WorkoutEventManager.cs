
using System.Collections.Generic;
using Website.Data;
using Website.Models;

namespace Website.Business
{
    public static class WorkoutEventManager
    {
        private static IWorkoutEventRepository _repository;
        private static IWorkoutEventRepository Repository => _repository ?? (_repository = new RunningRepository());

        public static void Insert(WorkoutEvent workoutEvent)
        {
            Repository.CreateActivity(workoutEvent);
        }

        public static WorkoutEvent Read(int id)
        {
            return Repository.ReadActivity(id);
        }

        public static void Update(WorkoutEvent workoutEvent)
        {
            Repository.UpdateActivity(workoutEvent);
        }

        public static void Delete(int id)
        {
            Repository.DeleteActivity(id);
        }

        public static IEnumerable<WorkoutEvent> ReadAll()
        {
            return Repository.GetAllActivities();
        }

        //TODO: Is there a better place to put this?
        public static IEnumerable<UnitOfMeasurement> GetUnitOfMeasurements()
        {
            return Repository.GetUnitOfMeasurements();
        }

        //TODO: Is there a better place to put this?
        public static IEnumerable<ActivityType> GetActivityTypes()
        {
            return Repository.GetActivityTypes();
        }
    }
}
