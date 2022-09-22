using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using com.tweetapp.DBContext;

namespace com.tweetapp.DBContext
{
    public class TweetsDatabaseSettings : ITweetsDatabaseSettings
    {
        public string TweetsCollection { get; set; }
        public string ConnectionString { get; set; }
        public string Database { get; set; }
    }
}
