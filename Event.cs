using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace MVCEventCalendar.Models
{
    public class Event
    {
        [Key]
        public int EventID { get; set; }
        public string Subject { get; set; }
        public string Description { get; set; }
        public string Location { get; set; }
        public System.DateTime Start { get; set; }
        public Nullable<System.DateTime> End { get; set; }
        public string ThemeColor { get; set; }
        public bool IsFullDay { get; set; }

        [RegularExpression("^[0-9]{8}$", ErrorMessage = "Please enter 8 digits in customer number")]
        [Required]
        public string User { get; set; }
        public string Invite { get; set; }


    }
}