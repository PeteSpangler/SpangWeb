using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SpangWebDotNet.Data.Models
{
    public class ResponsePostFullRequest
    {
        public int HabitId { get; set; }
        public string Feedback { get; set; }
        public string UserId { get; set; }
        public string UserName { get; set; }
        public DateTime Created { get; set; }
    }
}
