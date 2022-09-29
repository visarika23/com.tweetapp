using com.tweetapp.Models;
using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Text;

namespace com.tweetapp.Services
{
    public interface ITweetService
    {
        public Tweet PostTweet(Tweet tweet);
        public List<Tweet> ViewMyTweets(string email);
        public List<Tweet> ViewAllTweets();
        public bool UpdateTweet(Tweet tweet);
        public bool ReplyATweet(string id, string userName, TweetReply reply);
        public bool LikeTweet(string id, string userName);
        public bool UnLikeTweet(string id, string userName);
        public bool DeleteTweet(string id, string userName);
    }
}
