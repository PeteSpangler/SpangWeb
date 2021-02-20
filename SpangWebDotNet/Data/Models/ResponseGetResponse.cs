using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SpangWebDotNet.Data.Models
{
    public class ResponseGetResponse
    {
        public int ResponseId { get; set; }
        public string Feedback { get; set; }
        public string UserName { get; set; }
        public DateTime Created { get; set; }
    }
}
