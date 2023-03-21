using System;
using System.Collections.Generic;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace QuantU.Models{

    public class Portfolio {
        [BsonIgnoreIfDefault]
        public ObjectId _id {get; set;}
        public string? username {get; set;}
        public string? name {get; set;}
        public List<string>? stocks {get; set;}
        public List<int>? investments {get; set;}
        public List<decimal>? share{get; set;}


        public Portfolio() {

        }
    }



}