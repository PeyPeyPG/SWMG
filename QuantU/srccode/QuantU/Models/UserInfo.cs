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

    static String encrpyt = "gu4vajuic3keyb0ard86";

    public static UserInfo EncryptAlgo(UserInfo user) {
        StringBuilder buildUser = new StringBuilder();
        StringBuilder buildRecovery = new StringBuilder();
        StringBuilder buildEmail = new StringBuilder();


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


    public static string DecryptUsername(UserInfo user) {
        StringBuilder DecryptedUser = new StringBuilder();
        while(encrpyt.Length < user.username.Length) {
            encrpyt = encrpyt + "" + encrpyt;
        }

        for(int i = 0; i < user.username.Length; i++) {
            DecryptedUser.Append((char)(user.username[i] ^ encrpyt[i]));
        }
         Console.WriteLine(DecryptedUser.ToString());
        return DecryptedUser.ToString();
    }

    public static string DecryptEmail(UserInfo user) {
        StringBuilder DecryptedEmail = new StringBuilder();
        while(encrpyt.Length < user.email.Length) {
            encrpyt = encrpyt + "" + encrpyt;
        }

        for(int i = 0; i < user.email.Length; i++) {
            DecryptedEmail.Append((char)(user.email[i] ^ encrpyt[i]));
        }
         Console.WriteLine(DecryptedEmail.ToString());
        return DecryptedEmail.ToString();
    }

    public static string DecryptRecovery(UserInfo user) {
        StringBuilder DecryptedRecovery = new StringBuilder();
        while(encrpyt.Length < user.recoveryQ.Length) {
            encrpyt = encrpyt + "" + encrpyt;
        }

        for(int i = 0; i < user.recoveryQ.Length; i++) {
            DecryptedRecovery.Append((char)(user.recoveryQ[i] ^ encrpyt[i]));
        }
        Console.WriteLine(DecryptedRecovery.ToString());
        return DecryptedRecovery.ToString();
    }


    }
}