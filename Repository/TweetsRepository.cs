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

        public TweetsRepository(IConfiguration configuration)
        {
            _configuration = configuration;
            database = MongoConnection.GetDatabase();
            collection = database.GetCollection<Tweet>(_configuration.GetValue<string>("TweetCollection"));
        }

        public List<Tweet> GetAllTweets()
        {
            try
            {
                var tweets = collection.Find(_ => true).ToList();
                return tweets;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"\n Exception Occured: {ex.Message}");
                return null;
            }

        }

        public List<Tweet> GetMyTweets(string username)
        {
            try
            {
                var tweet = collection.Find(t => t.User.UserName == username).ToList();
                return tweet;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"\n Exception Occured: {ex.Message}");
                return null;
            }

        }
        public Tweet GetATweetByIdandUsername(ObjectId id, string userName)
        {
            var getTweet = collection.Find(t => t.Id == id && t.User.UserName == userName).FirstOrDefault();
            return getTweet;

        }

        public string AddTweet(Tweet tweet)
        {
            try
            {
                collection.InsertOne(tweet);
                return "\n Tweet Posted";
            }
            catch (Exception ex)
            {
                Console.WriteLine($"\n Exception Occured: {ex.Message}");
                return null;
            }

        }
        public string UpdateATweet(Tweet tweet)
        {
            try
            {
                var filter = Builders<Tweet>.Filter.Eq(p => p.TweetId, tweet.TweetId);
                var update = Builders<Tweet>.Update.Set(p => p.Message, tweet.Message);
                var result = collection.FindOneAndUpdate(filter, update);
                /*            var options = new UpdateOptions { IsUpsert = true };
                            collection.UpdateOne(filter, update, options);*/
                return "Success";
            }
            catch (Exception ex)
            {
                Console.WriteLine($"\n Exception Occured: {ex.Message}");
                return null;
            }           
        }


        public string LikeATweet(Tweet tweet)
        {
            try
            {
                var t = GetATweetByIdandUsername(tweet.Id, tweet.User.UserName);
                t.Likes++;
                var filter = Builders<Tweet>.Filter.Eq(p => p.TweetId, tweet.TweetId);
                var update = Builders<Tweet>.Update.Set(p => p, t);
                var result = collection.FindOneAndUpdate(filter, update);

                return "Success";
            }
            catch (Exception ex)
            {
                Console.WriteLine($"\n Exception Occured: {ex.Message}");
                return null;
            }
        }

        public string UnLikeATweet(Tweet tweet)
        {
            try
            {
                var t = GetATweetByIdandUsername(tweet.Id, tweet.User.UserName);
                t.Likes--;
                var filter = Builders<Tweet>.Filter.Eq(p => p.TweetId, tweet.TweetId);
                var update = Builders<Tweet>.Update.Set(p => p, t);
                var result = collection.FindOneAndUpdate(filter, update);

                return "Success";
            }
            catch (Exception ex)
            {
                Console.WriteLine($"\n Exception Occured: {ex.Message}");
                return null;
            }
        }

        public string ReplyATweet(Tweet tweet, TweetReply reply)
        {
            try
            {
                var t = collection.Find(t => t.TweetId == tweet.TweetId).FirstOrDefault();
                t.Reply.Add(reply);

                var filter = Builders<Tweet>.Filter.Eq(t => t.TweetId, tweet.TweetId);
                var update = Builders<Tweet>.Update.Set(p => p, t);

                var result = collection.FindOneAndUpdate(filter, update);

                return "Success";
            }
            catch (Exception ex)
            {
                Console.WriteLine($"\n Exception Occured: {ex.Message}");
                return null;
            }
        }

        public void DeleteATweet(ObjectId id, string username)
        {
            var response = collection.DeleteOne(t => t.Id == id && t.User.UserName == username);
        }
    }
}
