using Microsoft.Extensions.Configuration;
using MongoDB.Driver;
using System;

namespace com.tweetapp.DBConnection
{
    public class MongoConnection
    {
        private static IConfiguration configuration;
        public MongoConnection(IConfiguration config)
        {
            configuration = config;
        }
        public static IMongoDatabase GetDatabase()
        {
            try
            {
                //string connectionString = configuration.GetConnectionString("ConnectionString");
                //string dbName = configuration.GetValue<string>("Database");
                string connectionString = "mongodb+srv://visarika:visarika23@cluster0.45eue.mongodb.net/?retryWrites=true&w=majority";
                string dbName = "TweetAppDB";
                MongoClient client = new MongoClient(connectionString);
                return client.GetDatabase(dbName);
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
                return null;
            }
            
        }
    }
}
