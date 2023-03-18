using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;  
using System.Text;
using System.Security.Cryptography; 

namespace QuantU.Models{
    public class UserInfo {

        public string username {get; set;} = null!;
        public string email {get; set;} = null!;
        public string password {get; set;} = null!;
        [BsonElement("recovery question")]
        public string recoveryQ {get; set;} = null!;
        [BsonElement("recovery answer")]
        public string recoveryA{get;set;} = null!;



    public static UserInfo HashingAlgo(UserInfo user) {


         using (SHA256 sha256Hash = SHA256.Create())  {  
                // ComputeHash - returns byte array  
                byte[] bytes = sha256Hash.ComputeHash(Encoding.UTF8.GetBytes(user.password));  
  
                // Convert byte array to a string   
                StringBuilder builder = new StringBuilder();  
                for (int i = 0; i < bytes.Length; i++)   {  
                    builder.Append(bytes[i].ToString("x2"));  
                }  
                user.password = builder.ToString(); 
         }

         using (SHA256 sha256Hash = SHA256.Create())  {  
                // ComputeHash - returns byte array  
                byte[] bytes = sha256Hash.ComputeHash(Encoding.UTF8.GetBytes(user.recoveryA));  
  
                // Convert byte array to a string   
                StringBuilder builder = new StringBuilder();  
                for (int i = 0; i < bytes.Length; i++)   {  
                    builder.Append(bytes[i].ToString("x2"));  
                }  
                user.recoveryA = builder.ToString(); 
         }
    return user;
    }

    //fix this
    public static UserInfo EncryptAlgo(UserInfo user) {
        user.username= Convert.ToBase64String(Encoding.Unicode.GetBytes (user.username)) ;
        user.email = Convert.ToBase64String(Encoding.Unicode.GetBytes (user.email));
        user.recoveryQ = Convert.ToBase64String(Encoding.Unicode.GetBytes (user.recoveryQ));
        return user;
    }


    }
}