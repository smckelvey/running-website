using System;

namespace Website.Models
{
    public class WorkoutEvent
    {
        public int Id;
        public DateTime Date;
        public ActivityType Type;
        public string Duration;
        public decimal Distance;
        public UnitOfMeasurement Unit;
        public string Title;
        public string Notes;
        public User User;

        public WorkoutEvent()
        {

        }


    }
}
