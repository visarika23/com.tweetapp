using System;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace com.tweetapp.Models
{
    public class User
    {
        [BsonId]
        public ObjectId UserId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime DOB { get; set; }
        public string Gender { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        
        [BsonDefaultValue(false)]
        public bool IsLoggedIn { get; set; }
    }
}