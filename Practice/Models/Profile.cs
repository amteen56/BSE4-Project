using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Practice.Models
{
    public class Profile
    {
        public string Locations { get; set; }
        public string Full_Name { get; set; }
        public string Dateofbirth { get; set; }
        public string Gender { get; set; }
        public static string Username { get; set; }
        public static string oldUsername { get; set; }
        public string Mobile { get; set; }
        public string Email { get; set; }
        public string Address { get; set; }
        public static string Password { get; set; }
        public static string oldPassword { get; set; }
        public string UserName { get; set; }
        public string Passwords { get; set; }
        public bool RememberMe { get; set; }
    }
}