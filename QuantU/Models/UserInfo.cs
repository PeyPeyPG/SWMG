using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;  
using System.Text;
using System.Security.Cryptography; 

namespace QuantU.Models{
    public class UserInfo {

        /*  This class contains all info related to the User class objects.
            The code section below is both the constructor and initialization of class fields
            The BSON elemnts allow the field below it to be referred to as something else later in the code, used in the Index of the User View
            UserInfo items are encrypted and stored in an external Mongo database
        */  
        public string username {get; set;} = null!;
        public string email {get; set;} = null!;
        public string password {get; set;} = null!;
        [BsonElement("recovery question")]
        public string recoveryQ {get; set;} = null!;
        [BsonElement("recovery answer")]
        public string recoveryA{get;set;} = null!;



        /*  This function takes in a UserInfo object and hashes the data before it is stored
            This function is called in the UserController before the user is pushed to the database
            The data hashed is the password and the recovery question answer
            Inputs: a UserInfo object
            Outputs: a UserInfo object
            This code can likely be optimized later but I didn't find it to be a priority yet
        */
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



        /*  This function takes in an inputted password/recover answer and hashes it to be compared to the database
            Called on login to hash the password and compare, or when recovery answer is input
            Inputs: a string
            Outputs: a string
            This code can likely be optimized later but I didn't find it to be a priority yet
        */
    public static string HashedSingle(string pass) {
        String hashed;
        using (SHA256 sha256Hash = SHA256.Create())  {  
                // ComputeHash - returns byte array  
                byte[] bytes = sha256Hash.ComputeHash(Encoding.UTF8.GetBytes(pass));  
  
                // Convert byte array to a string   
                StringBuilder builder = new StringBuilder();  
                for (int i = 0; i < bytes.Length; i++)   {  
                    builder.Append(bytes[i].ToString("x2"));  
                }  
                 hashed = builder.ToString(); 
         }

         return hashed;
    }



    /*  This is the encryption key. It probably should not be stored in plaintext but here we are
        If this key is changed, be aware that none of the encrpyted data can be accessed until the string is changed back
        DO NOT CHANGE THE STRING WITHOUT CONSULTING SOMEBODY
    */
    static readonly String encrpyt = "gu4vajuic3keyb0ard86";



    /*  This function takes in a UserInfo object and encrypts the data before it is stored
        This function is called in the UserController before the user is pushed to the database
        The data encrypted is the username, email, and recovery question
        Inputs: a UserInfo object (also uses the encrypt string)
        Outputs: a UserInfo object
        This code can likely be optimized later but I didn't find it to be a priority yet
    */
    public static UserInfo EncryptAlgo(UserInfo user) {
        StringBuilder buildUser = new StringBuilder();
        StringBuilder buildRecovery = new StringBuilder();
        StringBuilder buildEmail = new StringBuilder();
        String encrpytCopy = encrpyt;


        while(encrpytCopy.Length < user.username.Length ||encrpytCopy.Length < user.recoveryQ.Length || encrpytCopy.Length < user.email.Length) {
            encrpytCopy = encrpytCopy + "" + encrpyt;
        }


        for(int i = 0; i < user.username.Length; i++) {
            buildUser.Append((char)(user.username[i] ^ encrpytCopy[i]));
        }
        Console.WriteLine(buildUser.ToString());
        user.username = buildUser.ToString();

        for(int i = 0; i < user.email.Length; i++) {
            buildEmail.Append((char)(user.email[i] ^ encrpytCopy[i]));
        }
        Console.WriteLine(buildEmail.ToString());
        user.email = buildEmail.ToString();

        for(int i = 0; i < user.recoveryQ.Length; i++) {
            buildRecovery.Append((char)(user.recoveryQ[i] ^ encrpytCopy[i]));
        }
        Console.WriteLine(buildRecovery.ToString());
        user.recoveryQ = buildRecovery.ToString();

        return user;

    }



    /*  This function takes in an encrypted UserInfo object and decrypts the item to be used/displayed
        As of creation, the code is not implemented anywhere but it has been tested
        The function only decrypts one item
        Can be used to decrypt a username, email, or recovery question
        Use on an item once typed in and compare to whats stored in the system
        Inputs: a UserInfo object (also uses the encrypt string)
        Outputs: a string
    */
    public static string DecryptSingle(string user) {
        StringBuilder DecryptedUser = new StringBuilder();
        String encrpytCopy = encrpyt;
        while(encrpytCopy.Length < user.Length) {
            encrpytCopy = encrpytCopy + "" + encrpyt;
        }

        for(int i = 0; i < user.Length; i++) {
            DecryptedUser.Append((char)(user[i] ^ encrpytCopy[i]));
        }
         Console.WriteLine(DecryptedUser.ToString());
        return DecryptedUser.ToString();
    }


    }
}