using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace QuantU.Models
{
    public class UserFinances
    {
        public ObjectId _id {get; set;}
        public string? username { get; set; }
        public double? netSalary { get; set; }
        public string? maritalStatus { get; set; }
        public string? interests { get; set; }
        public int? age { get; set; }
        public int? numberOfChildren { get; set; }
        public int eldestChildAge { get; set; }
        public bool TechnologyChecked { get; set; }
        public bool BankingChecked { get; set; }
        public bool AutomotiveChecked { get; set; }
        public List<Portfolio>? portfolioList { get; set; }

        public UserFinances()
        {}

        public UserFinances(string username){
            this.username = username;
            this.portfolioList = new List<Portfolio>();
        }
    }
}