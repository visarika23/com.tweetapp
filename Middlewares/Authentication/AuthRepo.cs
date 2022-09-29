using com.tweetapp.Models;
using com.tweetapp.Repository;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Security.Cryptography;

namespace com.tweetapp.Middlewares.Authentication
{
    public class AuthRepo : IAuthRepo
    {
        private readonly IConfiguration _config;
        static readonly log4net.ILog _log4net = log4net.LogManager.GetLogger(typeof(AuthRepo));
        public AuthRepo(IConfiguration config)
        {
            _config = config;
        }

        public string GenerateJSONWebToken(string username)
        {
            _log4net.Info("Generating token");

            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var claims = new[] {
                new Claim(JwtRegisteredClaimNames.Sub, username),
            };

            var token = new JwtSecurityToken(
              issuer: _config["Jwt:Issuer"],
              audience: _config["Jwt:Issuer"],
              null,
              expires: DateTime.Now.AddSeconds(30),
              signingCredentials: credentials);

            _log4net.Info("Token Is Generated!");

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        public string GetHash(string password)
        {
            // derive a 256-bit subkey (use HMACSHA1 with 10,000 iterations)
            ASCIIEncoding encoding = new ASCIIEncoding();
            byte[] salt = Encoding.UTF8.GetBytes("123");
            byte[] messageBytes = encoding.GetBytes(password);
            using (var hmacsha256 = new HMACSHA256(salt))
            {
                byte[] hashmessage = hmacsha256.ComputeHash(messageBytes);
                return Convert.ToBase64String(hashmessage);
            }
        }
    }
}
