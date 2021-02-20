using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace SpangWebDotNet.Data.Models
{
    public class HabitPostRequest
    {
        [Required]
        [StringLength(150)]
        public string DailyHabit { get; set; }
        [Required(ErrorMessage=
            "Please add more details for the intention of the habit you wish to adopt or have adopted.")]
        public string Intention { get; set; }
    }
}
