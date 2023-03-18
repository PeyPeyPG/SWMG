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
        StringBuilder buildUser = new StringBuilder();
        StringBuilder buildRecovery = new StringBuilder();
        StringBuilder buildEmail = new StringBuilder();

        String encrpyt = "gu4vajuic3keyb0ard86";
        while(encrpyt.Length < user.username.Length ||encrpyt.Length < user.recoveryQ.Length || encrpyt.Length < user.email.Length) {
            encrpyt = encrpyt + "" + encrpyt;
        }


        for(int i = 0; i < user.username.Length; i++) {
            buildUser.Append((char)(user.username[i] ^ encrpyt[i]));
        }
        Console.WriteLine(buildUser.ToString());
        user.username = buildUser.ToString();

        for(int i = 0; i < user.email.Length; i++) {
            buildEmail.Append((char)(user.email[i] ^ encrpyt[i]));
        }
        Console.WriteLine(buildEmail.ToString());
        user.email = buildEmail.ToString();

        for(int i = 0; i < user.recoveryQ.Length; i++) {
            buildRecovery.Append((char)(user.recoveryQ[i] ^ encrpyt[i]));
        }
        Console.WriteLine(buildRecovery.ToString());
        user.recoveryQ = buildRecovery.ToString();

        return user;

    }

    //update decrpytion to call for individual items
    public static UserInfo DecryptAlgo(UserInfo user) {
        return EncryptAlgo(user);
    }


    }
}