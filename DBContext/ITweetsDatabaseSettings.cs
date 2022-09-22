using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace com.tweetapp.DBContext
{
    public interface ITweetsDatabaseSettings
    {
        public string TweetsCollection { get; set; }
        public string ConnectionString { get; set; }
        public string Database{ get; set; }
    }
}
