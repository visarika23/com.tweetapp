using com.tweetapp.Repository;
using com.tweetapp.Models;
using MongoDB.Bson;
using System;
using System.Collections.Generic;

namespace com.tweetapp.Services
{
    public class TweetService : ITweetService
    {
        private ITweetsRepository _tweetsRepository;
        public TweetService(ITweetsRepository tweetsRepository)
        {
            _tweetsRepository = tweetsRepository;
        }
        
        public string PostTweet(Tweet tweet)
        {
            try
            {
                var response = _tweetsRepository.AddTweet(tweet);
                if (!string.IsNullOrEmpty(response))
                    return response;
                else
                    return null;
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
                return null;
            }
            
        }

        public List<Tweet> ViewAllTweets()
        {
            try
            {
                var tweets = _tweetsRepository.GetAllTweets();
                if (tweets.Count != 0)
                    return tweets;
                else
                    Console.WriteLine("\n There are no tweets yet.");
                return null;
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
                return null;
            }
            
        }

        public List<Tweet> ViewMyTweets(string username)
        {
            try
            {
                var tweets = _tweetsRepository.GetMyTweets(username);
                if (tweets.Count!= 0)
                    return tweets;
                else
                    Console.WriteLine("\n You have not posted any tweet yet.");
                return null;
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
                return null;
            }
            
        }

        public void UpdateTweet(Tweet tweet)
        {
            var getTweet = _tweetsRepository.GetATweetByIdandUsername(tweet.Id, tweet.User.UserName);
            if (getTweet != null)
            {
                var response =  _tweetsRepository.UpdateATweet(tweet);
            }
        }

        public void LikeTweet(ObjectId id, string userName)
        {
            var getTweet = _tweetsRepository.GetATweetByIdandUsername(id, userName);
            
            if (getTweet != null)
            {
                var response = _tweetsRepository.LikeATweet(getTweet);
            }
                
        }

        public void UnLikeTweet(ObjectId id, string userName)
        {
            var getTweet = _tweetsRepository.GetATweetByIdandUsername(id, userName);

            if (getTweet != null)
            {
                var response = _tweetsRepository.UnLikeATweet(getTweet);
            }

        }

        public void ReplyATweet(ObjectId id, string userName, TweetReply reply)
        {
            var getTweet = _tweetsRepository.GetATweetByIdandUsername(id, userName);

            if (getTweet != null)
            {
                var response = _tweetsRepository.ReplyATweet(getTweet,reply);
            }
        }

        public void DeleteTweet(ObjectId id, string userName)
        {
            var getTweet = _tweetsRepository.GetATweetByIdandUsername(id, userName);

            if (getTweet != null)
            {
                _tweetsRepository.DeleteATweet(id, userName);
            }

        }
    }
}
