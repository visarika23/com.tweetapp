using com.tweetapp.Models;
using MongoDB.Driver;
using System;

namespace com.tweetapp.DBContext
{
    public class MongoContext:IMongoContext
    {
        private IMongoDatabase _database;
        static readonly log4net.ILog _log = log4net.LogManager.GetLogger(typeof(MongoContext));
        public MongoContext(IMongoDatabase database)
        {
            _database = database;
        }
        public IMongoCollection<User> Users()
        {
            try
            {
                return _database.GetCollection<User>("Users");
            }
            catch(Exception ex)
            {
                _log.Info(ex.Message);
                return null;
            }
        }
        public IMongoCollection<Tweet> Tweets()
        {
            try
            {
                return _database.GetCollection<Tweet>("Tweets");
            }
            catch (Exception ex)
            {
                _log.Info(ex.Message);
                return null;
            }
        }
    }
}
