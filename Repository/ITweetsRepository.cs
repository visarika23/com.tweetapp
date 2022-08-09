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
        public bool AddTweet(Tweet tweet);
        public bool UpdateATweet(Tweet tweet);
        public bool LikeATweet(Tweet tweet);
        public bool UnLikeATweet(Tweet tweet);
        public bool ReplyATweet(Tweet tweet, TweetReply reply);
        public bool DeleteATweet(ObjectId id, string username);

    }
}
