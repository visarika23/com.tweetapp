using com.tweetapp.Models;
using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Text;

namespace com.tweetapp.Services
{
    public interface ITweetService
    {
        public string PostTweet(Tweet tweet);
        public List<Tweet> ViewMyTweets(string email);
        public List<Tweet> ViewAllTweets();
        public void UpdateTweet(Tweet tweet);
        public void ReplyATweet(ObjectId id, string userName, TweetReply reply);
        public void LikeTweet(ObjectId id, string userName);
        public void UnLikeTweet(ObjectId id, string userName);
        public void DeleteTweet(ObjectId id, string userName);
    }
}
