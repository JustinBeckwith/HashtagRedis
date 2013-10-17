using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HashtagRedis.ProcessMonitor.Redis
{
    public class RedisProcessTypeDescriptor : IProcessTypeDescriptor
    {
        public bool IsMatch(Process process)
        {
            throw new NotImplementedException();
        }
    }
}
