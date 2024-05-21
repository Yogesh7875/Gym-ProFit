using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Gym_Management_System.Models
{
    public class Visitors
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public DateTime VisitDate { get; set; }
        public DateTime DOB { get; set; }
        public string Address { get; set; }
        public string Gender { get; set; }
        public string EnquiryMode { get; set; }
    }
}