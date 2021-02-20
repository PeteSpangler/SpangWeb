using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace SpangWebDotNet.Data.Models
{
    public class ResponsePostRequest
    {
        [Required]
        public int? HabitId { get; set; }
        [Required]
        public string Feedback { get; set; }
    }
}
