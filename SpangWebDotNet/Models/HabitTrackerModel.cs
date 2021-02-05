using System;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SpangWebDotNet.Models
{
    public class Habits
    {
        public int Id { get; set; }

        [DataType(DataType.Date)]
        public DateTime Date { get; set; }
        [Display(Name = "Daily Habit")]
        public string Daily_Habit { get; set; }
        [Display(Name = "Is this habit Positive, Negative, or Neutral?")]
        public string Positive_Negative_Neutral { get; set; }

    }
}
