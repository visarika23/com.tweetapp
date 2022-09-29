using com.tweetapp.Models;

namespace com.tweetapp.Middlewares.Authentication
{
    public interface IAuthRepo
    {
        public string GenerateJSONWebToken(string username);
        public string GetHash(string password);
    }
}
