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
        static readonly log4net.ILog _log = log4net.LogManager.GetLogger(typeof(TweetService));

        public TweetService(ITweetsRepository tweetsRepository)
        {
            _tweetsRepository = tweetsRepository;
        }
        
        public Tweet PostTweet(Tweet tweet)
        {
            try
            {
                _log.Info("Posting a tweet");
                var response = _tweetsRepository.AddTweet(tweet);
                return response;
            }
            catch(Exception ex)
            {
                _log.Error(ex.Message);
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
                    _log.Error("\n There are no tweets yet.");
                return null;
            }
            catch(Exception ex)
            {
                _log.Info(ex.Message);
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
                    _log.Info("\n You have not posted any tweet yet.");
                return null;
            }
            catch(Exception ex)
            {
                _log.Error(ex.Message);
                return null;
            }
            
        }

        public bool UpdateTweet(Tweet tweet)
        {
            var getTweet = _tweetsRepository.GetATweetById(tweet.TweetId);
            if (getTweet != null)
            {
                var response =  _tweetsRepository.UpdateATweet(tweet);
                return response;
            }
            else
            {
                _log.Error("Tweet not found");
                return false;
            }
        }

        public bool LikeTweet(string id, string userName)
        {
            var getTweet = _tweetsRepository.GetATweetById(id);
            
            if (getTweet != null)
            {
                var response = _tweetsRepository.LikeATweet(getTweet);
                return response;
            }
            else
            {
                _log.Error("Tweet not found");
                return false;
            }

        }

        public bool UnLikeTweet(string id, string userName)
        {
            var getTweet = _tweetsRepository.GetATweetById(id);

            if (getTweet != null)
            {
                var response = _tweetsRepository.UnLikeATweet(getTweet);
                return response;
            }
            else
            {
                _log.Error("Tweet not found");
                return false;
            }

        }

        public bool ReplyATweet(string id, string userName, TweetReply reply)
        {
            var getTweet = _tweetsRepository.GetATweetById(id);

            if (getTweet != null)
            {
                var response = _tweetsRepository.ReplyATweet(getTweet,reply);
                return response;
            }
            else
            {
                _log.Error("Tweet not found");
                return false;
            }
        }

        public bool DeleteTweet(string id, string userName)
        {
            var getTweet = _tweetsRepository.GetATweetById(id);

            if (getTweet != null)
            {
                var response = _tweetsRepository.DeleteATweet(id, userName);
                return response;
            }
            else
            {
                _log.Error("Tweet not found");
                return false;
            }

        }
    }
}
