using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace redis.process
{
    public class RedisStartDetails
    {
        public int Port
        {
            get;
            set;
        }

        public string ConfigurationFilePath
        {
            get;
            set;
        }
    }
}
