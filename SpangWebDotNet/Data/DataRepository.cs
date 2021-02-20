using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using SpangWebDotNet.Data.Models;
using Dapper;
using Microsoft.Data.SqlClient;
using static Dapper.SqlMapper;
using System.Threading.Tasks;

namespace SpangWebDotNet.Data
{
    public class DataRepository : IDataRepository
    {
        private readonly string _connectionString;
        public DataRepository(IConfiguration configuration)
        {
            _connectionString = configuration["ConnectionStrings:DefaultConnection"];
        }

        public async Task<ResponseGetResponse> GetResponse(int responseId)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();
                return await connection.QueryFirstOrDefaultAsync<ResponseGetResponse>(@"EXEC dbo.Response_Get_ByResponseId @ResponseId = @ResponseId", new { ResponseId = responseId });
            }
        }

        public async Task<HabitGetSingleResponse> GetHabit(int habitId)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();
                using (GridReader results = await connection.QueryMultipleAsync(
                    @"EXEC dbo.Habit_GetSingle @HabitId = @HabitId; 
                      EXEC dbo.Response_Get_ByHabitId @HabitId = @HabitId",
                    new { HabitId = habitId }))
                {
                    var habit = (await results.ReadAsync<HabitGetSingleResponse>()).FirstOrDefault();
                    if (habit != null)
                    {
                        habit.Responses = (await results.ReadAsync<ResponseGetResponse>()).ToList();
                    }
                    return habit;
                }
            }
        }

        public async Task<IEnumerable<HabitGetManyResponse>> GetHabits()
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();
                return await connection.QueryAsync<HabitGetManyResponse>("EXEC dbo.Habit_GetMany");
            }
        }

        public async Task<IEnumerable<HabitGetManyResponse>> GetHabitsWithResponses()
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();

                var habitDictionary = new Dictionary<int, HabitGetManyResponse>();
                return (await connection.QueryAsync<HabitGetManyResponse, ResponseGetResponse, HabitGetManyResponse>("EXEC dbo.Habit_GetMany_WithResponses",
                  map: (q, a) =>
                  {
                      HabitGetManyResponse habit;

                      if (!habitDictionary.TryGetValue(q.HabitId, out habit))
                      {
                          habit = q;
                          habit.Responses = new List<ResponseGetResponse>();
                          habitDictionary.Add(habit.HabitId, habit);
                      }
                      habit.Responses.Add(a);
                      return habit;
                  },
                  splitOn: "HabitId"))
                  .Distinct()
                  .ToList();
            }
        }

        public async Task<IEnumerable<HabitGetManyResponse>> GetHabitsBySearch(string search)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();
                return await connection.QueryAsync<HabitGetManyResponse>(@"EXEC dbo.Habit_GetMany_BySearch 
                    @Search = @Search",
                    new { Search = search });
            }
        }

        public async Task<IEnumerable<HabitGetManyResponse>> GetHabitsBySearchWithPaging(string search, int pageNumber, int pageSize)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();
                var parameters = new { Search = search, PageNumber = pageNumber, PageSize = pageSize };
                return await connection.QueryAsync<HabitGetManyResponse>(@"EXEC dbo.Habit_GetMany_BySearch_WithPaging
                    @Search = @Search, @PageNumber = @PageNumber, @PageSize = @PageSize",
                    parameters);
            }
        }

        public async Task<IEnumerable<HabitGetManyResponse>> GetHabitsNoResponse()
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();
                return await connection.QueryAsync<HabitGetManyResponse>("EXEC dbo.Habit_GetNoResponses");
            }
        }

        public async Task<bool> HabitExists(int habitId)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();
                return await connection.QueryFirstAsync<bool>(@"EXEC dbo.Habit_Exists 
                    @HabitId = @HabitId",
                    new { HabitId = habitId });
            }
        }

        public async Task<HabitGetSingleResponse> PostHabit(HabitPostFullRequest habit)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();

                var habitId = await connection.QueryFirstAsync<int>(@"EXEC dbo.Habit_Post 
                    @DailyHabit = @DailyHabit, @Intention = @Intention, 
                    @UserId = @UserId,  @UserName = @UserName, 
                    @Created = @Created",
                    habit);
                return await GetHabit(habitId);
            }
        }

        public async Task<HabitGetSingleResponse> PutHabit(int habitId, HabitPutRequest habit)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();
                await connection.ExecuteAsync(@"EXEC dbo.Habit_Put 
                    @HabitId = @HabitId, @DailyHabit = @DailyHabit, @Intention = @Intention",
                    new { HabitId = habitId, habit.DailyHabit, habit.Intention });
                return await GetHabit(habitId);
            }
        }

        public async Task DeleteHabit(int habitId)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();
                await connection.ExecuteAsync(@"EXEC dbo.Habit_Delete 
                    @HabitId = @HabitId",
                    new { HabitId = habitId });
            }
        }

        public async Task<ResponseGetResponse> PostResponse(ResponsePostFullRequest answer)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();
                return await connection.QueryFirstAsync<ResponseGetResponse>(@"EXEC dbo.Response_Post 
                    @HabitId = @HabitId, @Feedback = @Feedback, @UserId = @UserId, @UserName = @UserName, @Created = @Created",
                    answer);
            }
        }
    }
}