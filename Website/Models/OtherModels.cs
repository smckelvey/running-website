using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Website.Models
{
    public class PaceEvent
    {
        public string Distance { get; set; }
        public string Time { get; set; }
        public string Pace { get; set; }
    }

    public class TimelineEvent
    {
        public string Distance { get; set; }
        public string Time { get; set; }
        public string Pace { get; set; }
        public string Race { get; set; }
        public string Notes { get; set; }
        public string Date { get; set; }
    }

    public class CalendarEvent
    {
        public int? Day { get; set; }
        public List<TimelineEvent> Events { get; set; }
    }

    public class TrainingSeason
    {
        public string Season { get; set; }
        public string Miles { get; set; }
    }

    public class Password
    {
        [Display(Name = "Password")]
        public string Value { get; set; }
    }
}