using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using Website.Models;

namespace Website.Data
{
    public class RunningRepository : IWorkoutEventRepository, IUserRepository
    {
        public IEnumerable<User> GetUsers()
        {
            using (var db = new Juggle4Food_RunningEntities())
            {
                return (from a in db.RunningPersons
                    orderby a.Id
                    select new User() { Id = a.Id, Name = a.Person }).ToList();
            }
        }

        public IEnumerable<UnitOfMeasurement> GetUnitOfMeasurements()
        {
            using (var db = new Juggle4Food_RunningEntities())
            {
                return (from a in db.RunningUnits
                    orderby a.Id
                    select new UnitOfMeasurement() {Id = a.Id, Name = a.Unit}).ToList();
            }
        }

        public IEnumerable<ActivityType> GetActivityTypes()
        {
            using (var db = new Juggle4Food_RunningEntities())
            {
                return (from a in db.RunningActivities
                    orderby a.Id
                    select new ActivityType() { Id = a.Id, Name = a.Activity }).ToList();
            }
        }

        public int CreateActivity(WorkoutEvent activity)
        {
            var runningEvent = MapRunningEvent(activity);

            using (var db = new Juggle4Food_RunningEntities())
            {
                db.RunningEvents.Add(runningEvent);
                db.SaveChanges();
            }

            return runningEvent.Id;
        }

        public WorkoutEvent ReadActivity(int id)
        {
            var workoutEvent = new WorkoutEvent();
            
            using (var db = new Juggle4Food_RunningEntities())
            {
                var runningEvent = db.RunningEvents.Find(id);
                if (runningEvent != null)
                {
                    workoutEvent = MapWorkoutEvent(runningEvent);
                }    
            }

            return workoutEvent;
        }

        public void UpdateActivity(WorkoutEvent activity)
        {
            var runningEvent = MapRunningEvent(activity);

            using (var db = new Juggle4Food_RunningEntities())
            {
                db.Entry(runningEvent).State = EntityState.Modified;
                db.SaveChanges();
            }
        }

        public void DeleteActivity(int id)
        {
            using (var db = new Juggle4Food_RunningEntities())
            {
                var runningEvent = db.RunningEvents.Find(id);

                if (runningEvent != null)
                {
                    db.RunningEvents.Remove(runningEvent);
                    db.SaveChanges();
                }
            }
        }

        public IEnumerable<WorkoutEvent> GetAllActivities()
        {
            using (var db = new Juggle4Food_RunningEntities())
            {
                return ConvertToWorkoutEvents((from e in db.RunningEvents
                    orderby e.Date
                    select e).ToList());
            }
        }

        public IEnumerable<WorkoutEvent> GetActivitesByType(string activity)
        {
            using (var db = new Juggle4Food_RunningEntities())
            {
                return ConvertToWorkoutEvents((from e in db.RunningEvents
                    where e.RunningActivity.Activity.Equals(activity, StringComparison.InvariantCultureIgnoreCase)
                    orderby e.Date
                    select e).ToList());
            }
        }

        public IEnumerable<WorkoutEvent> GetActivitiesByUser(int userId)
        {
            using (var db = new Juggle4Food_RunningEntities())
            {
                return ConvertToWorkoutEvents((from e in db.RunningEvents
                        where e.RunningPerson.Id.Equals(userId)
                        orderby e.Date
                        select e).ToList()); 
            }
        }

        public IEnumerable<WorkoutEvent> GetActivitiesByUser(string name)
        {
            using (var db = new Juggle4Food_RunningEntities())
            {
                return ConvertToWorkoutEvents((from e in db.RunningEvents
                    where e.RunningPerson.Person.Equals(name, StringComparison.InvariantCultureIgnoreCase)
                    orderby e.Date
                    select e).ToList());
            }
        }

        public IEnumerable<WorkoutEvent> GetActivitiesByDate(DateTime startDate, DateTime endDate)
        {
            using (var db = new Juggle4Food_RunningEntities())
            {
                return ConvertToWorkoutEvents((from e in db.RunningEvents
                        where e.Date >= startDate && e.Date <= endDate
                        orderby e.Date
                        select e).ToList());
            }
        }

        private static IEnumerable<WorkoutEvent> ConvertToWorkoutEvents(IEnumerable<RunningEvent> activities)
        {
            var results = new List<WorkoutEvent>();
            foreach (var activity in activities)
            {
                results.Add(MapWorkoutEvent(activity));
            }

            return results;
        }

        private static WorkoutEvent MapWorkoutEvent(RunningEvent e)
        {
            return new WorkoutEvent
            {
                Id = e.Id,
                Date = e.Date,
                Duration = e.Duration,
                Type = new ActivityType(e.RunningActivity.Id, e.RunningActivity.Activity),
                Distance = Convert.ToDecimal(e.Distance ?? 0),
                Title = e.Race,
                Notes = e.Notes,
                User = new User(e.RunningPerson.Id, e.RunningPerson.Person),
                Unit = new UnitOfMeasurement(e.RunningUnit.Id, e.RunningUnit.Unit)
            };
        }

        private static RunningEvent MapRunningEvent(WorkoutEvent e)
        {
            return new RunningEvent
            {
                Id = e.Id,
                Date = e.Date,
                Duration = e.Duration,
                ActivityId = e.Type.Id,
                Distance = Convert.ToDouble(e.Distance),
                Race = e.Title,
                Notes = e.Notes,
                PersonId = e.User.Id,
                UnitId = e.Unit.Id
            };
        }
    }
}
