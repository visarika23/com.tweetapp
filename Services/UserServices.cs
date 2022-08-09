using com.tweetapp.Repository;
using com.tweetapp.Models;
using System;
using System.Collections.Generic;
using com.tweetapp.Exceptions;
using Microsoft.Extensions.Configuration;
using MongoDB.Driver;
using com.tweetapp.DBConnection;

namespace com.tweetapp.Services
{
    public class UserServices : IUserServices
    {
        private DataValidationServices dataValidationServices=new DataValidationServices();
        private IUserRepository _userRepository;
        static readonly log4net.ILog _log = log4net.LogManager.GetLogger(typeof(UserServices));

        public UserServices(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public bool Register(User user)
        {
            try
            {
                _log.Info("Registering a user");

                if (string.IsNullOrEmpty(user.FirstName) || string.IsNullOrEmpty(user.Email) || string.IsNullOrEmpty(user.Gender) || string.IsNullOrEmpty(user.Password) || string.IsNullOrEmpty(user.UserName))
                {
                    _log.Info("Required fields were empty. Cannot register user");
                    throw new CustomException("Required fields should not be Empty");
                }
                var response = _userRepository.AddUser(user);

                return response;
            }
            catch (CustomException ex)
            {
                _log.Info($"\n Exception occured: {ex.Message}");
                return false;
            }

        }

        public string Login(Login login)
        {
            try
            {
                _log.Info("Logging in user");

                if (string.IsNullOrEmpty(login.UserName) || string.IsNullOrEmpty(login.Password))
                {
                    _log.Info("Required fields were empty. Cannot login user");
                    throw new CustomException("Required fields should not be Empty");
                }                  

                var getUser = _userRepository.GetByUsername(login.UserName);

                if (getUser == null)
                {
                    _log.Info("User not found with username" + login.UserName + ". Cannot login user");
                    return $"User not found with username {login.UserName}";
                }
                else if (getUser.Password == login.Password)
                {
                    var response = _userRepository.ChangeLoginStatus(login.UserName);
                    if (response)
                        return $"{login.UserName} is logged in";
                }
                else
                    _log.Info("\n Wrong Password.");                    
                
                return "Wrong Password";
            }
            catch(CustomException ex)
            {
               _log.Info($"\n Error occured: {ex.Message}" );
               return null;
            }           
        }   

        public string ForgotPassword(string username, string password)
        {
            try
            {
                _log.Info("User forgot passowrd.");
                if (string.IsNullOrEmpty(username))
                {
                    _log.Info("Required fields were empty");
                    throw new CustomException("Username should not be Empty");
                }
                
                var getUser= _userRepository.GetByUsername(username);

                if(getUser==null)
                {
                    _log.Info($"\n No account has been found with username {username}");
                    return "user not found";
                }
                else
                {
                    var response = _userRepository.ResetPassword(username, password);
                    if (response)
                        return $"password changed";
                    else
                        return $"Error occured";
                }                   
            }
            catch(Exception ex)
            {
                Console.WriteLine("\n "+ex.Message);
                return null;
            }
            
        }

        public List<User> GetAllUsers()
        {
            try
            {
                var users = _userRepository.GetAllUsers();
                if (users.Count != 0)
                    return users;
                else
                    _log.Info("\n No users yet.");
                    return null;
            }
            catch(Exception ex)
            {
                _log.Info("\n "+ex.Message);
                return null;
            }
            
        }

        public User GetAUser(string userName)
        {
            try
            {
                var user = _userRepository.GetByUsername(userName);
                if (user!= null)
                    return user;
                else
                {
                    _log.Info("\n User Not Found.");
                    return null;
                }                  
            }
            catch (Exception ex)
            {
                _log.Info("\n " + ex.Message);
                return null;
            }

        }

        public bool Logout(string username)
        {
            try
            {
                var response= _userRepository.ChangeLoginStatus(username);
                return response;
            }
            catch(Exception ex)
            {
                _log.Info("\n "+ex.Message);
                return false;
            }
            
        }
    }
}
