using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using Website.Business;
using Website.Common.Extensions;

namespace Website.Models
{
    public class WorkoutEventViewModel
    {
        public int Id { get; set; }
        public DateTime Date { get; set; }
        public int ActivityId { get; set; }
        public string Duration { get; set; }
        public decimal Distance { get; set; }
        public int UnitId { get; set; }
        public string Title { get; set; }
        public string Notes { get; set; }
        public int PersonId { get; set; }
    }
}