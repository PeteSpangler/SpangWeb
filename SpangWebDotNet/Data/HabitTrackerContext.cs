using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using SpangWebDotNet.Models;

namespace SpangWebDotNet.Data
{
    public class HabitTrackerContext : DbContext
    {
        public HabitTrackerContext (DbContextOptions<HabitTrackerContext> options)
            : base(options)
        {

        }
        public DbSet<Habits> Habit { get; set; }
    }
}
