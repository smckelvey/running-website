using System;
using System.Collections.Generic;
using System.Linq;
using Website.Models;
using Website.Data;

namespace Website.Business
{
    public class GarminImporter
    {
        private static IWorkoutEventRepository Repository { get; set; }

        public GarminImporter()
        {
            Repository = new RunningRepository();
        }

        public void Import(IEnumerable<IEnumerable<string>> rawData, int personId)
        {
            IEnumerable<WorkoutEvent> parsedEvents = CreateWorkoutEvents(rawData);

            //Filter events to only ones we want to enter
            List<WorkoutEvent> filteredEvents = FilterEvents(parsedEvents, personId);

            //Insert the events
            InsertEvents(filteredEvents);
        }

        private void InsertEvents(IEnumerable<WorkoutEvent> filteredEvents)
        {
            foreach (var currentEvent in filteredEvents)
            {
                WorkoutEventManager.Insert(currentEvent);
            }
        }

        private List<WorkoutEvent> FilterEvents(IEnumerable<WorkoutEvent> importedEvents, int personId)
        {
            var cleansedEvents = new List<WorkoutEvent>();

            foreach (var currentEvent in importedEvents)
            {
                var newEvent = new WorkoutEvent
                {
                    Type = new ActivityType(1, "Running"),
                    Unit = new UnitOfMeasurement(1, "mi"),
                    User = MembershipProvider.ReadAll().FirstOrDefault(u => u.Id == personId),
                    Date = currentEvent.Date.Date,
                    Distance = currentEvent.Distance,
                    Duration = currentEvent.Duration
                };

                if (currentEvent.Title.ToLower() != "richardson running" && currentEvent.Title.ToLower() != "dallas running")
                {
                    newEvent.Title = currentEvent.Title;
                }

                //Only add if there's not a similar activity on the same day
                var matchingEventExists = WorkoutEventManager.ReadAll().Any(
                        e =>
                            e.User.Id == newEvent.User.Id &&
                            e.Date.Date == newEvent.Date.Date &&
                            e.Distance < newEvent.Distance + 1 &&
                            e.Distance > newEvent.Distance - 1);

                if (!matchingEventExists)
                {
                    cleansedEvents.Add(newEvent);
                }
            }

            return cleansedEvents;
        }

        private IEnumerable<WorkoutEvent> CreateWorkoutEvents(IEnumerable<IEnumerable<string>> rawData)
        {
            var results = new List<WorkoutEvent>();

            //Get the first row column names into a list so we can find the correct data in any order
            var headerRow = rawData.First().ToArray();
            var columnIndexes = new Dictionary<string, int>();
            for (int i = 0; i < headerRow.Count(); i++)
            {
                columnIndexes[headerRow[i].ToLower()] = i;
            }
            
            //validate that the required columns are present somewhere in the file
            string[] requiredColumns = { "activity type", "date", "title", "distance", "time" };
            foreach (string columnName in requiredColumns)
            {
                if (!columnIndexes.ContainsKey(columnName))
                {
                    throw new ArgumentException("File does not contain the required fields: " + requiredColumns);
                }
            }

            for (int i = 1; i < rawData.Count(); i++)
            {
                //Processing row
                //Activity Type,Date,Favorite,Title,Distance,Calories,Time
                var fields = rawData.ElementAt(i).ToArray();
                if (fields != null && fields[columnIndexes["activity type"]].ToLower() == "running")
                {
                    var workoutEvent = new WorkoutEvent
                    {
                        Date = Convert.ToDateTime(fields[columnIndexes["date"]]),
                        Title = fields[columnIndexes["title"]],
                        Distance = Convert.ToDecimal(fields[columnIndexes["distance"]]),
                        Duration = fields[columnIndexes["time"]]
                    };

                    results.Add(workoutEvent);
                }
            }

            return results;
        }
    }
}
