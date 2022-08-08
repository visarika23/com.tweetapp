using com.tweetapp.Models;
using MongoDB.Driver;

namespace com.tweetapp.DBContext
{
    public class MongoContext:IMongoContext
    {
        private IMongoDatabase _database;
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
            catch
            {
                return null;
            }
        }
        public IMongoCollection<Tweet> Tweets()
        {
            try
            {
                return _database.GetCollection<Tweet>("Tweets");
            }
            catch
            {
                return null;
            }
        }
    }
}
