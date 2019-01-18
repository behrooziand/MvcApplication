using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MvcApplication.Models
{
    public class UserProfile
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }

        public string UserName { get; set; }
        
        
        public string DisplayName { get; set; }

        public string EmployeeId { get; set; }



        //public string Manager { get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }
        public List<string> Groups { get; set; }
    }
}