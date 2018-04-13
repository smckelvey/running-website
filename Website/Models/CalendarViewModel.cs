using System;
using System.Collections.Generic;
using System.Linq;
using Website.Common.Extensions;
using Website.Business;

namespace Website.Models
{
    public class CalendarViewModel
    {
        public string CurrentMonthName { get; set; }
        public int CurrentMonth { get; set; }
        public int CurrentYear { get; set; }

        public List<CalendarEvent> MonthlyActivities { get; set; }

        public void Populate(string name, DateTime startDate)
        {
            CurrentMonthName = startDate.ToString("MMMM");
            CurrentMonth = startDate.Month;
            CurrentYear = startDate.Year;

            var daysOfMonth = BuildDays(name, startDate);

            MonthlyActivities = daysOfMonth;
        }

        private List<CalendarEvent> BuildDays(string userName, DateTime startDate)
        {
            var metrics = new MonthlyData(userName, startDate);

            var daysOfMonth = new List<CalendarEvent>();

            int startOffset = (int)startDate.DayOfWeek; //Sunday = 0
            for (int i = 0; i < startOffset; i++)
            {
                daysOfMonth.Add(new CalendarEvent()); //Blanks to pad before the first starts
            }

            //Actual days in the month
            var endDate = startDate.AddMonths(1).AddDays(-1);
            for (int i = 1; i <= endDate.Day; i++)
            {
                var dayEvents = new CalendarEvent
                {
                    Day = i,
                    Events = new List<TimelineEvent>()
                };

                var day = i;
                var daysActivities = metrics.Activities.Where(e => e.Date.Date == new DateTime(startDate.Year, startDate.Month, day).Date);
                foreach (var activity in daysActivities)
                {
                    var myEvent = new TimelineEvent();
                    var speed = Math.Round((activity.Duration.ToMinutes() / activity.Distance.ToMiles(activity.Unit.Name)), 2);
                    myEvent.Pace = speed.ToDuration();
                    myEvent.Time = activity.Duration;
                    myEvent.Distance = activity.Distance.ToMiles(activity.Unit.Name).ToFriendlyDistance();
                    myEvent.Race = activity.Title;
                    myEvent.Notes = activity.Notes;

                    dayEvents.Events.Add(myEvent);
                }

                daysOfMonth.Add(dayEvents);
            }

            //Pad any extra spaces to multiples of 7
            for (int i = 0; i < daysOfMonth.Count % 7; i++)
            {
                daysOfMonth.Add(new CalendarEvent());
            }

            return daysOfMonth;
        }
    }
}