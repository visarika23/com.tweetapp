using com.tweetapp.Models;
using System.Collections.Generic;
using com.tweetapp.DBConnection;
using MongoDB.Driver;
using Microsoft.Extensions.Configuration;
using System;
using log4net;

namespace com.tweetapp.Repository
{
    public class UserRepository : IUserRepository
    {

        private static IConfiguration _configuration;
        private readonly IMongoCollection<User> collection;
        private IMongoDatabase database;
        private List<User> allUsers = new List<User>();
        static readonly log4net.ILog _log4net = log4net.LogManager.GetLogger(typeof(UserRepository));

        public UserRepository(IConfiguration configuration)
        {
            _configuration = configuration;
            database = MongoConnection.GetDatabase();
            collection = database.GetCollection<User>(_configuration.GetValue<string>("UserCollection"));
        }

        public List<User> GetAllUsers()
        {
            try
            {
                allUsers = collection.Find(_ => true).ToList();
                return allUsers;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"\n Exception Occured: {ex.Message}");
                return null;
            }
        }
        public User GetByUsername(string username)
        {
            try
            {
                var user = collection.Find(s => s.UserName == username).FirstOrDefault();
                return user;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"\n Exception Occured: {ex.Message}");
                return null;
            }
        }

        public string AddUser(User user)
        {
            try
            {
                collection.InsertOne(user);
                return $"\n Hi {user.FirstName} {user.LastName}, Your Account has been created Successfully.";
            }
            catch (Exception ex)
            {
                Console.WriteLine($"\n Exception Occured: {ex.Message}");
                return null;
            }
        }

        public string ResetPassword(string username, string password)
        {
            try
            {
                collection.FindOneAndUpdate<User>(Builders<User>.Filter.Eq(s => s.UserName, username),
                Builders<User>.Update.Set(s => s.Password, password));
                return $" Password Updated";
            }
            catch (Exception ex)
            {
                Console.WriteLine($"\n Exception Occured: {ex.Message}");
                return null;
            }
        }

        public string ChangeLoginStatus(string username)
        {
            try
            {
                collection.FindOneAndUpdate<User>(Builders<User>.Filter.Eq(s => s.UserName, username),
                Builders<User>.Update.Set(s => s.IsLoggedIn, true));
                return "Success";
            }
            catch (Exception ex)
            {
                Console.WriteLine($"\n Exception Occured: {ex.Message}");
                return null;
            }
        }
    }
}
