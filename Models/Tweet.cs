using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;

namespace com.tweetapp.Models
{
    public class Tweet
    {
        [BsonId]
        public ObjectId Id { get; set; }

/*        [BsonRepresentation(BsonType.ObjectId)]
        public string TweetId
        {
            get { return Convert.ToString(Id); }
            //set { Id = MongoDB.Bson.ObjectId.Parse(value); }
        }*/
        public string Message { get; set; }
        public string UserName { get; set; }
        public string FirstName { get; set; }
        public DateTime TimeStamp { get; set; }
        public int Likes { get; set; }
        public List<TweetReply> Reply { get; set; }
    }
}
