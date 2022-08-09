using com.tweetapp.Models;
using System.Collections.Generic;

namespace com.tweetapp.Repository
{
    public interface IUserRepository
    {
        public List<User> GetAllUsers();
        public User GetByUsername(string username);
        public bool AddUser(User user);
        public bool ResetPassword(string username, string password);
        public bool ChangeLoginStatus(string username);
    }
}