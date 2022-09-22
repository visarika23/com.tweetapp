using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using com.tweetapp.Models;
using Microsoft.AspNetCore.Mvc;
using com.tweetapp.Repository;
using MongoDB.Bson;
using com.tweetapp.Services;
using Microsoft.AspNetCore.Cors;
using com.tweetapp.Middlewares.Authentication;
using Microsoft.AspNetCore.Authorization;

namespace com.tweetapp.Controllers
{
    [EnableCors()]
    [Route("api/v1.0/[controller]")]
    [ApiController]
    /*    [Authorize]*/
    [AllowAnonymous]
    public class TweetsController : ControllerBase
    {
        private ITweetService tweetService;
        private IUserServices userServices;
        private IAuthRepo _userAuth;
        private static string token;
        static readonly log4net.ILog _log = log4net.LogManager.GetLogger(typeof(TweetsController));

        public TweetsController(ITweetService tweetService, IUserServices userServices, IAuthRepo userAuth)
        {
            this.tweetService = tweetService;
            this.userServices = userServices;
            _userAuth = userAuth;   
        }

        [AllowAnonymous]
        [HttpPost("login")]
        public IActionResult Login([FromBody] Login credentials)
        {
            try
            {
                var response = userServices.Login(credentials);
                if(response.Contains($"{credentials.UserName} is logged in"))
                {
                    token = _userAuth.GenerateJSONWebToken(credentials.UserName);
                    return Ok(token);
                }
                else if (response.Contains("User not found"))
                {
                    return StatusCode(404, new { msg = "User not found" });
                }
                else
                    return StatusCode(400, new { msg = "Password is incorrect" });

            }
            catch (Exception ex)
            {
                _log.Error(ex.Message);
                return StatusCode(500, new { msg = "Internal server Error", err = ex });
            }

        }

        [HttpGet("all")]
        public IActionResult GetAllTweets()
        {
            try
            {
                var response = tweetService.ViewAllTweets();
                if(response == null)
                {
                    return StatusCode(204, new { msg = "No tweets yet" });
                }
                return Ok(response);
            }
            catch(Exception ex)
            {
                _log.Error(ex.Message);
                return StatusCode(500, new {msg="Internal server Error",err=ex });
            }
            
        }

        [HttpGet("users/all")]
        public IActionResult GetAllUsers()
        {
            try
            {
                var response = userServices.GetAllUsers();
                if (response == null)
                {
                    return StatusCode(204, new { msg = "No users registered yet" });
                }
                return Ok(response);
 
            }
            catch (Exception ex)
            {
                _log.Error(ex.Message);
                return StatusCode(500, new { msg = "Internal server Error", err = ex });
            }

        }

        [HttpGet("user/search/{username}")]
        public IActionResult GetAUser(string username)
        {
            try
            {
                var response = userServices.GetAUser(username);
                if (response == null)
                {
                    return StatusCode(204, new { msg = "No user found" });
                }
                return Ok(response);
            }
            catch (Exception ex)
            {
                _log.Error(ex.Message);
                return StatusCode(500, new { msg = "Internal server Error", err = ex });
            }

        }

        [HttpGet("{username}")]
        public IActionResult GetAllTweetsOfAUser(string username)
        {
            try
            {
                var response = tweetService.ViewMyTweets(username);
                if (response == null)
                {
                    return StatusCode(204, new { msg = "You don't have any tweets yet" });
                }
                return Ok(response);
            }
            catch(Exception ex)
            {
                _log.Error(ex.Message);
                return StatusCode(404, new {msg="User Not Found",err=ex });
            }
            
        }

        [AllowAnonymous]
        [HttpPost("register")]
        public IActionResult Register([FromBody] User user)
        {
            try
            {
                var response = userServices.Register(user);
                if (response)
                {
                    _log.Info("User added Succesfully");
                    token = _userAuth.GenerateJSONWebToken(user.UserName);
                    return Ok(token);
                }
                else
                    return StatusCode(400, new { msg = "User cannot be registered" });
            }
            catch (Exception ex)
            {
                _log.Error(ex.Message);
                return StatusCode(500, new { msg = "Internal server Error", err = ex });
            }

        }

        [HttpPost("{username}/add")]
        public IActionResult PostNewTweet(string username,[FromBody] Tweet tweet)
        {
            try
            {
                tweet.UserName = username;
                var response = tweetService.PostTweet(tweet);
                if (response)
                {
                    _log.Info("Tweet posted Succesfully");
                    return Ok(new { msg = "Tweet posted successfully" });
                }
                else
                    return StatusCode(400, new { msg = "Tweet could not be posted" });
            }
            catch(Exception ex)
            {
                _log.Error(ex.Message);
                return StatusCode(500, new { msg = "Internal server error.",err = ex });
            }
            
        }

        [HttpPost("{username}/reply/{id}")]
        public IActionResult Reply(string username, string id, [FromBody] TweetReply reply)
        {
            try
            {
                var response = tweetService.ReplyATweet(ObjectId.Parse(id), username, reply);
                if (response)
                {
                    _log.Info("Reply added to tweet");
                    return Ok(new { msg = "Reply posted" });
                }
                else
                    return StatusCode(400, new { msg = "Reply cannot be added to tweet" });
            }
            catch (Exception ex)
            {
                _log.Error(ex.Message);
                return StatusCode(500, new { msg = "Internal server error.", err = ex });
            }
        }

        [HttpPut("{username}/forgot")]
        public IActionResult Forgot(string username, [FromBody] string password)
        {
            try
            {
                var response = userServices.ForgotPassword(username, password);
                if (response.Contains("user not found"))
                {
                    return StatusCode(400, new { msg = "User not found" });
                }
                else if(response.Contains("password changed"))
                {
                    return Ok(new { response = "Your password has been updated" });
                }
                else
                {
                    return StatusCode(400, new { msg = "Password cannot be updated" });
                }
            }
            catch (Exception ex)
            {
                _log.Error(ex.Message);
                return StatusCode(500, new { msg = "Internal server Error", err = ex });
            }

        }

        [HttpPut("{username}/update/{id}")]
        public IActionResult UpdateAtweet(string username,string id, [FromBody] Tweet tweet)
        {
            try
            {
                tweet.Id = ObjectId.Parse(id);
                tweet.UserName = username;
                var response = tweetService.UpdateTweet(tweet);
                if (response)
                {
                    _log.Info("Tweet is updated");
                    return Ok(new { msg = "Tweet is updated" });
                }
                else
                    return StatusCode(400, new { msg = "Tweet cannot be updated" });
            }
            catch(Exception ex)
            {
                _log.Error(ex.Message);
                return StatusCode(500, new { msg = "Internal server error.", err = ex });
            }
        }

        [HttpPut("{username}/like/{id}")]
        public IActionResult Like(string username, string id)
        {
            try
            {       
                var response = tweetService.LikeTweet(ObjectId.Parse(id), username);
                if (response)
                {
                    _log.Info("Tweet is successfully Liked");
                    return Ok(new { msg = "Successfully Liked" });
                }
                else
                    return StatusCode(400, new { msg = "Tweet cannot be liked" });
            }
            catch (Exception ex)
            {
                _log.Error(ex.Message);
                return StatusCode(500, new { msg = "Internal server error.", err = ex });
            }

        }

        [HttpPut("{username}/unlike/{id}")]
        public IActionResult UnLike(string username, string id)
        {
            try
            {                
                var response = tweetService.UnLikeTweet(ObjectId.Parse(id), username);
                if (response)
                {
                    _log.Info("Tweet is successfully unliked");
                    return Ok(new { msg = "Successfully UnLiked" });
                }
                else
                    return StatusCode(400, new { msg = "Tweet cannot be unliked" });
            }
            catch (Exception ex)
            {
                _log.Error(ex.Message);
                return StatusCode(500, new { msg = "Internal server error.", err = ex });
            }

        }

        [HttpDelete("{username}/delete/{id}")]
        public IActionResult Delete(string username, string id)
        {
            try
            {       
                var response = tweetService.DeleteTweet(ObjectId.Parse(id), username);
                if (response)
                {
                    _log.Info("Tweet is successfully deletd");
                    return Ok(new { msg = "Successfully deleted" });
                }
                else
                    return StatusCode(400, new { msg = "Tweet cannot be deleted" });
            }
            catch (Exception ex)
            {
                _log.Error(ex.Message);
                return StatusCode(500, new { msg = "Internal server error.", err = ex });
            }

        }
    }
}
