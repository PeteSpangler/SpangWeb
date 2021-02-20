using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace SpangWebDotNet.Data.Models
{
    public class HabitPutRequest
    {
        [StringLength(150)]
        public string DailyHabit { get; set; }
        public string Intention { get; set; }
    }
}
