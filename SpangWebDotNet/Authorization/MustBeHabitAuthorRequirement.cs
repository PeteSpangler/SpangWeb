using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;

namespace SpangWebDotNet.Authorization
{
    public class MustBeHabitAuthorRequirement:IAuthorizationRequirement
    {
        public MustBeHabitAuthorRequirement()
        {

        }
    }
}
