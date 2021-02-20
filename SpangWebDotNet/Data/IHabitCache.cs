using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SpangWebDotNet.Data.Models;

namespace SpangWebDotNet.Data
{
    public interface IHabitCache
    {
        HabitGetSingleResponse Get(int habitId);
        void Remove(int habitId);
        void Set(HabitGetSingleResponse habit);
    }
}
