using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SpangWebDotNet.Data.Models
{
    public class HabitPostFullRequest
    {
        public string DailyHabit { get; set; }
        public string Intention { get; set; }
        public string UserId { get; set; }
        public string UserName { get; set; }
        public DateTime Created { get; set; }
    }
}
