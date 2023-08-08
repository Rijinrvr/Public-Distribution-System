using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace PersonDistributionSystem.Models
{
    public class LoginModel
    {
       public LoginModel() { 
        
            Isadmin = false;
        }


        public int Id { get; set; }

        public int UserId { get; set; }

        public string Username { get; set; }

        public string Password { get; set; }

        public bool Isadmin { get; set; }

    }
}