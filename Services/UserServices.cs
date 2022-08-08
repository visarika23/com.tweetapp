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

        public UserServices(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public string Register(User user)
        {
            try
            {
                if (string.IsNullOrEmpty(user.FirstName) || string.IsNullOrEmpty(user.Email) || string.IsNullOrEmpty(user.Gender) || string.IsNullOrEmpty(user.Password) || string.IsNullOrEmpty(user.UserName))
                {
                    throw new CustomException("Required fields should not be Empty");
                }
                var response = _userRepository.AddUser(user);

                if (!string.IsNullOrEmpty(response))
                    return response;
                else
                    return null;
            }
            catch (CustomException ex)
            {
                Console.WriteLine($"\n {ex.Message}");
                return null;
            }

        }

        public string Login(Login login)
        {
            try
            {
                if (string.IsNullOrEmpty(login.UserName) || string.IsNullOrEmpty(login.Password))
                {
                    //Console.WriteLine($"\n Email and Password are required fields.");
                    //return null;
                    throw new CustomException("Required fields should not be Empty");
                }                  

                var getUser = _userRepository.GetByUsername(login.UserName);

                if (getUser == null)
                    //throw new CustomException($" No account has been found with mail id {user.email}");
                    //return null;
                    return $" No account has been found with mail id {login.UserName}";

                else if (getUser.Password == login.Password)
                {
                    var response = _userRepository.ChangeLoginStatus(login.UserName);
                    if (response == "Success")
                        return $" Hi {getUser.FirstName} {getUser.LastName}, You are Logged in now.";
                }
                else
                    Console.WriteLine("\n Wrong Password.");                    
                
                return null;
            }
            catch(CustomException ex)
            {
               Console.WriteLine($"\n {ex.Message}" );
               return null;
            }           
        }   

        public string ForgotPassword(string username, string password)
        {
            try
            {
                if (string.IsNullOrEmpty(username))
                {
                    Console.WriteLine($"\n Email is a required field.");
                    return null;
                }
                
                var getUser= _userRepository.GetByUsername(username);

                if(getUser==null)
                {
                    Console.WriteLine($"\n No account has been found with mail id {username}");
                    return null;
                }
                else
                {
                    var response = _userRepository.ResetPassword(username, password);
                    if (response == "Success")
                        return $"Your password has been changed!";
                    else
                        return $"Error occured. Passowrd couldn't be updated";
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
                    Console.WriteLine("\n No users yet.");
                    return null;
            }
            catch(Exception ex)
            {
                Console.WriteLine("\n "+ex.Message);
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
                    Console.WriteLine("\n User Not Found.");
                    return null;
                }                  
            }
            catch (Exception ex)
            {
                Console.WriteLine("\n " + ex.Message);
                return null;
            }

        }

        public bool Logout(string username)
        {
            try
            {
                var response= _userRepository.ChangeLoginStatus(username);
                if(response.Equals("Success"))
                    return true;
                else
                    return false;
            }
            catch(Exception ex)
            {
                Console.WriteLine("\n "+ex.Message);
                return false;
            }
            
        }
    }
}
