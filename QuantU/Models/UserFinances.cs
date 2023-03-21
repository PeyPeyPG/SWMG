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
        public bool TechnologyChecked { get; set; }
        public bool BankingChecked { get; set; }
        public bool AutomotiveChecked { get; set; }
        public List<List<StockData>>? portfolioList { get; set; }
    

        public UserFinances(string username){
            this.username = username;
            this.portfolioList = new List<List<StockData>>();
        }
    }
}
