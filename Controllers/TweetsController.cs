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

namespace com.tweetapp.Controllers
{
    [EnableCors()]
    [Route("api/v1.0/[controller]")]
    [ApiController]
    public class TweetsController : ControllerBase
    {
        public ITweetService tweetService;
        private IUserServices userServices;
        public TweetsController(ITweetService tweetService, IUserServices userServices)
        {
            this.tweetService = tweetService;
            this.userServices = userServices;
        }

        // GET: api/Tweets
        [HttpGet("all")]
        public ObjectResult GetAllTweets()
        {
            try
            {
                return Ok(tweetService.ViewAllTweets());
            }
            catch(Exception ex)
            {
                return StatusCode(500, new {msg="Internal server Error",err=ex });
            }
            
        }

        // GET: api/Users/all
        [HttpGet("users/all")]
        public ObjectResult GetAllUsers()
        {
            try
            {
                return Ok(userServices.GetAllUsers());
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { msg = "Internal server Error", err = ex });
            }

        }

        [HttpGet("user/search/{username}")]
        public ObjectResult GetAUser([FromQuery] string userName)
        {
            try
            {
                return Ok(userServices.GetAUser(userName));
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { msg = "Internal server Error", err = ex });
            }

        }

        [HttpGet("{username}")]
        public ObjectResult GetAllTweetsOfAUser([FromQuery] string userName)
        {
            try
            {
                return Ok(tweetService.ViewMyTweets(userName));
            }
            catch(Exception ex)
            {
                return StatusCode(404, new {msg="User Not Found",err=ex });
            }
            
        }

        [HttpPost("login")]
        public ObjectResult Login([FromBody] Login credentials)
        {
            try
            {
                var response = userServices.Login(credentials);
                if (!string.IsNullOrEmpty(response))
                    return Ok(response);
                else
                    return StatusCode(404, new { msg = "Username/password is incorrect" });

            }
            catch (Exception ex)
            {
                return StatusCode(500, new { msg = "Internal server Error", err = ex });
            }

        }

        // POST: api/Users
        [HttpPost("register")]
        public ObjectResult Register([FromBody] User user)
        {
            try
            {
                userServices.Register(user);
                return Ok(new { msg = "User added successfully" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { msg = "Internal server Error", err = ex });
            }

        }


        // POST: api/Tweets
        [HttpPost("{username}/add")]
        public ObjectResult PostNewTweet(string username,[FromBody] Tweet tweet)
        {
            try
            {
                tweet.User.UserName = username;
                tweetService.PostTweet(tweet);
                return Ok(new { msg = "Successfully added." });
            }
            catch(Exception ex)
            {
                return StatusCode(500, new { msg = "Internal server error.",err = ex });
            }
            
        }

        [HttpPost("{username}/reply/{id}")]
        public ObjectResult Reply([FromQuery] string username, string id, [FromBody] TweetReply reply)
        {
            try
            {
                tweetService.ReplyATweet(ObjectId.Parse(id), username, reply);
                return Ok(new { msg = "Replied added to the tweet." });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { msg = "Internal server error.", err = ex });
            }
        }

        [HttpPut("{username}/forgot")]
        public ObjectResult Forgot([FromQuery] string username, [FromBody] string password)
        {
            try
            {
                var response = userServices.ForgotPassword(username, password);
                if (response == null)
                {
                    return Ok(new { response = "User Not Found" });
                }
                else
                {
                    return Ok(new { response = "Success" });
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { msg = "Internal server Error", err = ex });
            }

        }

        // PUT: api/Tweets/5
        [HttpPut("{username}/update/{id}")]
        public ObjectResult UpdateAtweet([FromQuery] string username,string id, [FromBody] Tweet tweet)
        {
            try
            {
                tweet.Id = ObjectId.Parse(id);
                tweet.User.UserName = username;
                tweetService.UpdateTweet(tweet);
                return Ok(new { msg = "Successfully Updated" });
            }
            catch(Exception ex)
            {
                return StatusCode(500, new { msg = "Internal server error.", err = ex });
            }
        }

        [HttpPut("{username}/like/{id}")]
        public ObjectResult Like([FromQuery] string username, string id)
        {
            try
            {
                tweetService.LikeTweet(ObjectId.Parse(id), username);
                return Ok(new { msg = "Successfully Liked." });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { msg = "Internal server error.", err = ex });
            }

        }

        [HttpPut("{username}/unlike/{id}")]
        public ObjectResult UnLike([FromQuery] string username, string id)
        {
            try
            {
                tweetService.UnLikeTweet(ObjectId.Parse(id), username);
                return Ok(new { msg = "Successfully UnLiked." });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { msg = "Internal server error.", err = ex });
            }

        }

        // DELETE: api/ApiWithActions/5
        [HttpDelete("{username}/delete/{id}")]
        public ObjectResult Delete(string username, string id)
        {
            try
            {
                tweetService.DeleteTweet(ObjectId.Parse(id), username);
                return Ok(new { msg = "Successfully Deleted." });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { msg = "Internal server error.", err = ex });
            }

        }
    }
}
