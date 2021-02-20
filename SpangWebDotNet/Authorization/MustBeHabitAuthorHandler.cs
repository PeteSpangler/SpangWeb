using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using SpangWebDotNet.Data;

namespace SpangWebDotNet.Authorization
{
    public class MustBeHabitAuthorHandler : AuthorizationHandler<MustBeHabitAuthorRequirement>
    {
        private readonly IDataRepository _dataRepository;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public MustBeHabitAuthorHandler(IDataRepository dataRepository, IHttpContextAccessor httpContextAccessor)
        {
            _dataRepository = dataRepository;
            _httpContextAccessor = httpContextAccessor;
        }

        protected async override Task HandleRequirementAsync(AuthorizationHandlerContext context, MustBeHabitAuthorRequirement requirement)
        {
            if (!context.User.Identity.IsAuthenticated)
            {
                context.Fail();
                return;
            }

            var habitId = _httpContextAccessor.HttpContext.Request.RouteValues["habitId"];
            int habitIdAsInt = Convert.ToInt32(habitId);
            var userId = context.User.FindFirst(ClaimTypes.NameIdentifier).Value;
            var habit = await _dataRepository.GetHabit(habitIdAsInt);
            if (habit == null)
            {
                // let it through so the controller can return a 404
                context.Succeed(requirement);
                return;
            }
            if (habit.UserId != userId)
            {
                context.Fail();
                return;
            }
            context.Succeed(requirement);
        }
    }
}
