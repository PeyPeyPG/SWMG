using System;
using System.Collections.Generic;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace QuantU.Models{

    public class Portfolio {
        public List<string>? stocks {get; set;}

        public List<int>? investments {get; set;}


        public Portfolio() {
            
        }
    }



}