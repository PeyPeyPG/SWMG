using System.Collections;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace QuantU.Models;

public class StockData{
    public ObjectId _id {get; set;}
    public string? ticker { get; set; }
    public decimal? value { get; set; }
    public string? name { get; set; }

/*
    public AppData(DataPartition dp){
        ticker = dp.ticker;
        value = dp.value;
        companyName = dp.companyName;
    }

    public void updateValue(decimal newValue) {
        value = newValue;
    }

    public decimal getValue() {
        return value;
    }
    */
}