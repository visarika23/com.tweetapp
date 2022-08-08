using System;
using System.Collections.Generic;
using System.Text;

namespace com.tweetapp.Exceptions
{
    [Serializable]
    public class CustomException:Exception
    {
        public CustomException()
        {

        }
        public CustomException(string message):base(String.Format($" Exception Occured: {message}"))
        {

        }
    }
}
