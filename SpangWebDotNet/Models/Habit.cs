using System;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SpangWebDotNet.Models
{
    public class Habit
    {
        public int Id { get; set; }

        [DataType(DataType.Date)]
        public DateTime Date { get; set; }
        [Display(Name = "Daily Habit")]
        public string DailyHabit { get; set; }
        [Display(Name = "Is the intention of this habit Positive, Negative, or Neutral?")]
        public string Intention { get; set; }
        [Display(Name ="Are you stacking this with another habit?")]
        public string Feedback { get; set; }

    }
}
