using System.Diagnostics;
using HashtagRedis;

class Program
{
    static void Main(string[] args)
    {
        Trace.Assert(null != BusinessLogic.CreateInstance("test"));
        // TODO: More Smoke tests
    }
}
