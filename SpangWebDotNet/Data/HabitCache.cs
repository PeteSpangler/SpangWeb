using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Caching.Memory;
using SpangWebDotNet.Data.Models;


namespace SpangWebDotNet.Data
{
    public class HabitCache : IHabitCache
    {
        private MemoryCache _cache { get; set; }
        public HabitCache()
        {
            _cache = new MemoryCache(new MemoryCacheOptions
            {
                SizeLimit = 100
            });
        }

        private string GetCacheKey(int habitId) => $"Habit-{habitId}";
        
        public HabitGetSingleResponse Get(int habitId)
        {
            HabitGetSingleResponse habit;
            _cache.TryGetValue(GetCacheKey(habitId), out habit);
            return habit;
        }

        public void Set(HabitGetSingleResponse habit)
        {
            var cacheEntryOptions = new MemoryCacheEntryOptions().SetSize(1);
            _cache.Set(GetCacheKey(habit.HabitId), habit, cacheEntryOptions);
        }

        public void Remove(int habitId)
        {
            _cache.Remove(GetCacheKey(habitId));
        }
    }
}
