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
        static readonly log4net.ILog _log = log4net.LogManager.GetLogger(typeof(UserRepository));

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
                _log.Info("Fetching all users from database");

                allUsers = collection.Find(_ => true).ToList();
                return allUsers;
            }
            catch (Exception ex)
            {
                _log.Info($"\n Exception Occured: {ex.Message}");
                return null;
            }
        }
        public User GetByUsername(string username)
        {
            try
            {
                _log.Info("Fetching a user from database");

                var user = collection.Find(s => s.UserName == username).FirstOrDefault();
                return user;
            }
            catch (Exception ex)
            {
                _log.Info($"\n Exception Occured: {ex.Message}");
                return null;
            }
        }

        public bool AddUser(User user)
        {
            try
            {
                _log.Info("Adding a user to database");

                collection.InsertOne(user);
                return true;
            }
            catch (Exception ex)
            {
                _log.Info($"\n Exception Occured: {ex.Message}");
                return false;
            }
        }

        public bool ResetPassword(string username, string password)
        {
            try
            {
                _log.Info("Resetting user password");

                collection.FindOneAndUpdate<User>(Builders<User>.Filter.Eq(s => s.UserName, username),
                Builders<User>.Update.Set(s => s.Password, password));
                return true;
            }
            catch (Exception ex)
            {
                _log.Info($"\n Exception Occured: {ex.Message}");
                return false;
            }
        }

        public bool ChangeLoginStatus(string username)
        {
            try
            {
                _log.Info("Changing log in status of user to true");

                collection.FindOneAndUpdate<User>(Builders<User>.Filter.Eq(s => s.UserName, username),
                Builders<User>.Update.Set(s => s.IsLoggedIn, true));
                return true;
            }
            catch (Exception ex)
            {
                _log.Info($"\n Exception Occured: {ex.Message}");
                return false;
            }
        }
    }
}
