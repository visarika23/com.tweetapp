using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;

namespace com.tweetapp.Models
{
    public class TweetReply
    {
        [BsonId]
        public ObjectId ReplyId { get; set; }
        public string ReplyMessage { get; set; }
        public User User { get; set; }
        public DateTime TimeStamp { get; set; }
    }
}
