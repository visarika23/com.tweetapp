using com.tweetapp.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace com.tweetapp.Services
{
    public interface IUserServices
    {
        public bool Register(User user);
        public string Login(Login cred);
        public string ForgotPassword(string username, string password);
        public List<User> GetAllUsers();
        public User GetAUser(string userName);
        public bool Logout(string email);

    }
}
