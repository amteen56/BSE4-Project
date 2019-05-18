using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Practice.Models
{
    public class Feedback
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public string Against_Username { get; set; }
        public string Report_Type { get; set; }
        public string Report_Data { get; set; }
        public DateTime Date { get; set; }
        public static int id { get; set; }
        public static string username { get; set; }
        public static string Againstusername { get; set; }
        public static string type { get; set; }
        public static string Report { get; set; }
        public static DateTime date { get; set; }
    }
}