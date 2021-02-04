using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SpangWebDotNet.Models
{
    public class HabitTrackerModel
    {
        public string Id { get; set; }
        public int Date { get; set; }
        public string Habit { get; set; }
        public bool Completed { get; set; }

    }
}
