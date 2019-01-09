using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace MVCEventCalendar.Models
{
    public class Users
    {

        [Required]
        //[StringLength(50, MinimumLength = 8)]
        [RegularExpression("^[0-9]{8}$", ErrorMessage = "Please enter 8 digits in customer number")]
        [Key]
        public string ID { get; set; }
        [Required]
        public string passworld { get; set; }

        [RegularExpression("^[0-3]{1}$")]
        public string type { get; set; }
        [Required]
        [EmailAddress(ErrorMessage = "Invalid Email Address")]
        public string email { get; set; }
        public string Worker { get; set; }

    }
}