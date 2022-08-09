using Microsoft.Extensions.Configuration;
using MongoDB.Driver;
using System;

namespace com.tweetapp.DBConnection
{
    public class MongoConnection
    {
        private static IConfiguration configuration;
        static readonly log4net.ILog _log = log4net.LogManager.GetLogger(typeof(MongoConnection));
        public MongoConnection(IConfiguration config)
        {
            configuration = config;
        }
        public static IMongoDatabase GetDatabase()
        {
            try
            {
                _log.Info("Setting up DB connection");
                //string connectionString = configuration.GetConnectionString("ConnectionString");
                //string dbName = configuration.GetValue<string>("Database");
                string connectionString = "mongodb+srv://visarika:visarika23@cluster0.45eue.mongodb.net/?retryWrites=true&w=majority";
                string dbName = "TweetAppDB";
                MongoClient client = new MongoClient(connectionString);
                return client.GetDatabase(dbName);
            }
            catch(Exception ex)
            {
                _log.Info(ex.Message);
                return null;
            }
            
        }
    }
}
