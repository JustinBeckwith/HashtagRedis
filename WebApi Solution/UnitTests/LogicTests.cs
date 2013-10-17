using Microsoft.VisualStudio.TestTools.UnitTesting;
using redis.logic;

namespace UnitTests
{
    [TestClass]
    public class LogicTests
    {
        [TestMethod]
        public void CreateInstance()
        {
            var instanceId = "testInstance";
            var instance = Manager.CreateInstance(instanceId);
            Assert.IsNotNull(instance);
            Assert.AreEqual(instanceId, instance.instanceId);
            Assert.AreEqual("redpolo", instance.password);
            Assert.AreEqual(2001, instance.port);
            Assert.AreNotEqual(0, instance.processId);
            Assert.AreEqual("redis://" + instanceId + ":redpolo@hashtagredis.cloudapp.net:2001/", instance.connectionString);
        }
    }
}
