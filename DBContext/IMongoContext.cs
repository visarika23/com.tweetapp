using com.tweetapp.Models;
using MongoDB.Driver;

namespace com.tweetapp.DBContext
{
    public interface IMongoContext
    {
        public IMongoCollection<User> Users();
        public IMongoCollection<Tweet> Tweets();
    }
}