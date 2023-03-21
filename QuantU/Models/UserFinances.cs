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

        //User Interests checkboxes
        public bool TechnologyChecked { get; set; }
        public bool BankingChecked { get; set; }
        public bool AutomotiveChecked { get; set; }

        public List<List<StockData>>? portfolioList { get; set; }

        //constructor for null values
        public UserFinances()
        {}

        //peytons shit
        public UserFinances(string username)
        {
            this.username = username;
            this.portfolioList = new List<List<StockData>>();
        }
    }
}
