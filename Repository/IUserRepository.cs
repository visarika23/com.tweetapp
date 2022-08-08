using com.tweetapp.Models;
using System.Collections.Generic;

namespace com.tweetapp.Repository
{
    public interface IUserRepository
    {
        public List<User> GetAllUsers();
        public User GetByUsername(string username);
        public string AddUser(User user);
        public string ResetPassword(string username, string password);
        public string ChangeLoginStatus(string username);
    }
}