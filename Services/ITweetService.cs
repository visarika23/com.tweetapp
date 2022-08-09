using com.tweetapp.Models;
using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Text;

namespace com.tweetapp.Services
{
    public interface ITweetService
    {
        public bool PostTweet(Tweet tweet);
        public List<Tweet> ViewMyTweets(string email);
        public List<Tweet> ViewAllTweets();
        public bool UpdateTweet(Tweet tweet);
        public bool ReplyATweet(ObjectId id, string userName, TweetReply reply);
        public bool LikeTweet(ObjectId id, string userName);
        public bool UnLikeTweet(ObjectId id, string userName);
        public bool DeleteTweet(ObjectId id, string userName);
    }
}
