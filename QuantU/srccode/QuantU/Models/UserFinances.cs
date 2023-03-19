using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace QuantU.Models
{
    public class UserFinances
    {
        public string? username { get; set; }
        public double? netSalary { get; set; }
        public string? maritalStatus { get; set; }
        public string? interests { get; set; }
        public int? age { get; set; }
        public Portfolio? portfolio {get; set; }

        public UserFinances(string Username, double salary, string marry, string inter, int Age) {
            username = Username;
            netSalary = salary;
            maritalStatus = marry;
            interests = inter;
            age = Age;
        }
    }
}