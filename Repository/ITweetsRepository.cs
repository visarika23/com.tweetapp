using com.tweetapp.Models;
using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Text;

namespace com.tweetapp.Repository
{
    public interface ITweetsRepository
    {
        public List<Tweet> GetAllTweets();
        public List<Tweet> GetMyTweets(string username);
        public Tweet GetATweetByIdandUsername(ObjectId id, string userName);
        public string AddTweet(Tweet tweet);
        public string UpdateATweet(Tweet tweet);
        public string LikeATweet(Tweet tweet);
        public string UnLikeATweet(Tweet tweet);
        public string ReplyATweet(Tweet tweet, TweetReply reply);
        public void DeleteATweet(ObjectId id, string username);

    }
}
