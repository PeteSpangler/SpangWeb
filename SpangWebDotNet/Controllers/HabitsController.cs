using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SpangWebDotNet.Data;
using SpangWebDotNet.Data.Models;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using Microsoft.Extensions.Configuration;
using System.Net.Http;
using System.Text.Json;

namespace SpangWebDotNet.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HabitsController : ControllerBase
    {
        private readonly IDataRepository _dataRepository;
        private readonly IHabitCache _cache;
        private readonly IHttpClientFactory _clientFactory;
        private readonly string _auth0UserInfo;

        public HabitsController(IDataRepository dataRepository, IHabitCache habitCache, IHttpClientFactory clientFactory, IConfiguration configuration)
        {
            _dataRepository = dataRepository;
            _cache = habitCache;
            _clientFactory = clientFactory;
            _auth0UserInfo = $"{configuration["Auth0:Authority"]}userinfo";
        }

        [HttpGet]
        public async Task<IEnumerable<HabitGetManyResponse>> GetHabits(string search, bool includeResponses, int page = 1, int pageSize = 20)
        {
            if (string.IsNullOrEmpty(search))
            {
                if (includeResponses)
                {
                    return await _dataRepository.GetHabitsWithResponses();
                }
                else
                {
                    return await _dataRepository.GetHabits();
                }
            }
            else
            {
                return await _dataRepository.GetHabitsBySearchWithPaging(search, page, pageSize);
            }
        }

        [HttpGet("noresponses")]
        public async Task<IEnumerable<HabitGetManyResponse>> GetHabitsNoResponse()
        {
            return await _dataRepository.GetHabitsNoResponse();
        }

        [HttpGet("{habitId}")]
        public async Task<ActionResult<HabitGetSingleResponse>> GetHabit(int habitId)
        {
            var habit = _cache.Get(habitId);
            if (habit == null)
            {
                habit = await _dataRepository.GetHabit(habitId);
                if (habit == null)
                {
                    return NotFound();
                }
                _cache.Set(habit);
            }
            return habit;
        }

        [Authorize]
        [HttpPost]
        public async Task<ActionResult<HabitGetSingleResponse>> PostHabit(HabitPostRequest habitPostRequest)
        {
            var savedHabit = await _dataRepository.PostHabit(new HabitPostFullRequest
            {
                DailyHabit = habitPostRequest.DailyHabit,
                Intention = habitPostRequest.Intention,
                UserName = await GetUserName(),
                UserId = User.FindFirst(ClaimTypes.NameIdentifier).Value,
                Created = DateTime.UtcNow
            });
            return CreatedAtAction(nameof(GetHabit), new
            {
                habitId = savedHabit.HabitId
            }, savedHabit);
        }

        [Authorize(Policy = "MustBeHabitAuthor")]
        [HttpPut("{habitId}")]
        public async Task<ActionResult<HabitGetSingleResponse>> PutHabit(int habitId, HabitPutRequest habitPutRequest)
        {
            var habit = await _dataRepository.GetHabit(habitId);
            if (habit == null)
            {
                return NotFound();
            }
            habitPutRequest.DailyHabit = string.IsNullOrEmpty(habitPutRequest.DailyHabit) ? habit.DailyHabit : habitPutRequest.DailyHabit;
            habitPutRequest.Intention = string.IsNullOrEmpty(habitPutRequest.Intention) ? habit.Intention : habitPutRequest.Intention;
            var savedHabit = await _dataRepository.PutHabit(habitId, habitPutRequest);
            _cache.Remove(savedHabit.HabitId);
            return savedHabit;
        }

        [Authorize(Policy = "MustBeHabitAuthor")]
        [HttpDelete("{habitId}")]
        public async Task<ActionResult> DeleteHabit(int habitId)
        {
            var habit = await _dataRepository.GetHabit(habitId);
            if (habit == null)
            {
                return NotFound();
            }
            await _dataRepository.DeleteHabit(habitId);
            _cache.Remove(habitId);
            return NoContent();
        }

        [Authorize(Policy = "MustBeHabitAuthor")]
        [HttpPost("response")]
        public async Task<ActionResult<ResponseGetResponse>> PostResponse(ResponsePostRequest responsePostRequest)
        {
            var habitExists = await _dataRepository.HabitExists(responsePostRequest.HabitId.Value);
            if (!habitExists)
            {
                return NotFound();
            }
            var savedResponse = await _dataRepository.PostResponse(new ResponsePostFullRequest
            {
                HabitId = responsePostRequest.HabitId.Value,
                Feedback = responsePostRequest.Feedback,
                UserId = User.FindFirst(ClaimTypes.NameIdentifier).Value,
                UserName = await GetUserName(),
                Created = DateTime.UtcNow
            });

            _cache.Remove(responsePostRequest.HabitId.Value);

            return savedResponse;
        }

        private async Task<string> GetUserName()
        {
            var request = new HttpRequestMessage(HttpMethod.Get, _auth0UserInfo);
            request.Headers.Add("Authorization", Request.Headers["Authorization"].First());

            var client = _clientFactory.CreateClient();

            var response = await client.SendAsync(request);

            if (response.IsSuccessStatusCode)
            {
                var jsonContent = await response.Content.ReadAsStringAsync();
                var user = JsonSerializer.Deserialize<User>(jsonContent, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
                return user.Name;
            }
            else
            {
                return "";
            }
        }
    }
}