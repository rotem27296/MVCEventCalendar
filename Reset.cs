using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace MVCEventCalendar.Models
{
    public class Reset
    {

        [Required]
        [RegularExpression("^[0-9]{8}$", ErrorMessage = "Please enter 8 digits in customer number")]
        [Key]
        public string ID { get; set; }
        public string hash { get; set; }

    }
}