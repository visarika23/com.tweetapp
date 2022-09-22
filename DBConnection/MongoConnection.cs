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
        public static IMongoDatabase GetDatabase(string dbName)
        {
            try
            {
                _log.Info("Setting up DB connection");
                string connectionString = configuration.GetConnectionString(dbName);
                MongoClient client = new MongoClient(connectionString);
                return client.GetDatabase(dbName);
            }
            catch(Exception ex)
            {
                _log.Error(ex.Message);
                return null;
            }
            
        }
    }
}
