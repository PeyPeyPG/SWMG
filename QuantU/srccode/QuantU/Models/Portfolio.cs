using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace QuantU.Models{

    public class Portfolio {
        public list<string>? stocks {get; set;}

        public list<int>? investments {get; set;}
    }



}