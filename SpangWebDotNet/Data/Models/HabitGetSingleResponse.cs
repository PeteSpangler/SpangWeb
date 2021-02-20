using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SpangWebDotNet.Data.Models
{
    public class HabitGetSingleResponse
    {
        public int HabitId { get; set; }
        public string DailyHabit { get; set; }
        public string Intention { get; set; }
        public string UserName { get; set; }
        public string UserId { get; set; }
        public DateTime Created { get; set; }
        public IEnumerable<ResponseGetResponse> Responses { get; set; }
    }
}
