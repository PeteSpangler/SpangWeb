using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SpangWebDotNet.Data.Models;

namespace SpangWebDotNet.Data
{
    public interface IDataRepository
    {
        Task<IEnumerable<HabitGetManyResponse>> GetHabits();
        Task<IEnumerable<HabitGetManyResponse>> GetHabitsWithResponses();
        Task<IEnumerable<HabitGetManyResponse>> GetHabitsBySearch(string search);
        Task<IEnumerable<HabitGetManyResponse>> GetHabitsBySearchWithPaging(string search, int pageNumber, int pageSize);
        Task<IEnumerable<HabitGetManyResponse>> GetHabitsNoResponse();
        Task<HabitGetSingleResponse> GetHabit(int habitId);
        Task<bool> HabitExists(int habitId);
        Task<ResponseGetResponse> GetResponse(int responseId);
        Task<HabitGetSingleResponse> PostHabit(HabitPostFullRequest habit);
        Task<HabitGetSingleResponse> PutHabit(int habitId, HabitPutRequest habit);
        Task DeleteHabit(int habitId);
        Task<ResponseGetResponse> PostResponse(ResponsePostFullRequest answer);
    }
}
