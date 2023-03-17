using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
namespace QuantU.Models{
    public class UserInfo {

        public string username {get; set;} = null!;
        public string email {get; set;} = null!;
        public string password {get; set;} = null!;
        [BsonElement("recovery question")]
        public string recoveryQ {get; set;} = null!;
        [BsonElement("recovery answer")]
        public string recoveryA{get;set;} = null!;



        /*public User(string username, string email, string password, string recoveryQ, string recoveryA){
            this.username = username;
            this.email = email;
            this.password = password;
            this.recoveryQ = recoveryQ;
            this.recoveryA = recoveryA;
        }*/

    }
}