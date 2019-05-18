using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Practice.Models
{
    public class UserRecords
    {
        public string UserName { get; set; }
        public int Total_Wins { get; set; }
        public int Total_Played { get; set; }
        public float WinPercentage { get; set; }
    }
}