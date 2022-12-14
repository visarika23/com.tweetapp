using com.tweetapp.DBContext;
using com.tweetapp.DBConnection;
using com.tweetapp.Models;
using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using Microsoft.Extensions.Configuration;

namespace com.tweetapp.Repository
{
    public class TweetsRepository : ITweetsRepository
    {
        private static IConfiguration _configuration;
        private readonly IMongoCollection<Tweet> collection;
        private IMongoDatabase database;
        private List<Tweet> allTweets = new List<Tweet>();
        static readonly log4net.ILog _log = log4net.LogManager.GetLogger(typeof(TweetsRepository));

        public TweetsRepository(ITweetsDatabaseSettings settings)
        {
            var client = new MongoClient(settings.ConnectionString);
            var database = client.GetDatabase(settings.Database);
            collection = database.GetCollection<Tweet>(settings.TweetsCollection);
        }

        public List<Tweet> GetAllTweets()
        {
            try
            {
                _log.Info("Fetching all tweets from database");

                var tweets = collection.Find(_ => true).ToList();
                return tweets;
            }
            catch (Exception ex)
            {
                _log.Error($"\n Exception Occured: {ex.Message}");
                return null;
            }

        }

        public List<Tweet> GetMyTweets(string username)
        {
            try
            {
                _log.Info("Fetching a user's all tweets from database");

                allTweets = collection.Find(t => t.UserName == username).ToList();
                return allTweets;
            }
            catch (Exception ex)
            {
                _log.Error($"\n Exception Occured: {ex.Message}");
                return null;
            }

        }
        public Tweet GetATweetById(string id)
        {
            _log.Info("Fetching a tweet from database");
            var getTweet = collection.Find(t => t.TweetId == id ).FirstOrDefault();
            return getTweet;

        }

        public Tweet AddTweet(Tweet tweet)
        {
            try
            {
                _log.Info("adding tweets to database"); 

                collection.InsertOne(tweet);
                return tweet;
            }
            catch (Exception ex)
            {
                _log.Error($"\n Exception Occured: {ex.Message}");
                return null;
            }

        }
        public bool UpdateATweet(Tweet tweet)
        {
            try
            {
                _log.Info("Updating a tweet");
                var filter = Builders<Tweet>.Filter.Eq(p => p.TweetId, tweet.TweetId);
                var update = Builders<Tweet>.Update.Set(p => p.Message, tweet.Message);
                var result = collection.FindOneAndUpdate(filter, update);
                /*            var options = new UpdateOptions { IsUpsert = true };
                            collection.UpdateOne(filter, update, options);*/
                return true;
            }
            catch (Exception ex)
            {
                _log.Error($"\n Exception Occured: {ex.Message}");
                return false;
            }           
        }


        public bool LikeATweet(Tweet tweet)
        {
            try
            {
                _log.Info("Liking a tweet");
                var t = GetATweetById(tweet.TweetId);
                t.Likes++;
                var filter = Builders<Tweet>.Filter.Eq(p => p.TweetId, tweet.TweetId);
                var update = Builders<Tweet>.Update.Set(p => p.Likes, t.Likes);
                var result = collection.FindOneAndUpdate(filter, update);

                return true;
            }
            catch (Exception ex)
            {
                _log.Error($"\n Exception Occured: {ex.Message}");
                return false;
            }
        }

        public bool UnLikeATweet(Tweet tweet)
        {
            try
            {
                _log.Info("Unliking a tweet");
                var t = GetATweetById(tweet.TweetId);
                t.Likes--;
                var filter = Builders<Tweet>.Filter.Eq(p => p.TweetId, tweet.TweetId);
                var update = Builders<Tweet>.Update.Set(p => p, t);
                var result = collection.FindOneAndUpdate(filter, update);

                return true;
            }
            catch (Exception ex)
            {
                _log.Error($"\n Exception Occured: {ex.Message}");
                return false;
            }
        }

        public bool ReplyATweet(Tweet tweet, TweetReply reply)
        {
            try
            {
                _log.Info("replying a tweet");
                //var t = collection.Find(t => t.Id == tweet.Id).FirstOrDefault();

                if(tweet.Reply != null)
                {
                    tweet.Reply.Add(reply);
                }
                else
                {
                    tweet.Reply = new List<TweetReply>();
                    tweet.Reply.Add(reply);
                }
               

                var filter = Builders<Tweet>.Filter.Eq(t => t.TweetId, tweet.TweetId);
                var update = Builders<Tweet>.Update.Set(p => p.Reply, tweet.Reply);

                var result = collection.FindOneAndUpdate(filter, update);


/*                var options = new UpdateOptions { IsUpsert = true };
                collection.UpdateOne(filter, update, options);*/

                return true;
            }
            catch (Exception ex)
            {
                _log.Error($"\n Exception Occured: {ex.Message}");
                return false;
            }
        }

        public bool DeleteATweet(string id, string username)
        {
            _log.Info("Deleting a tweet");
            var response = collection.DeleteOne(t => t.TweetId == id && t.UserName == username);
            return true;
        }
    }
}
