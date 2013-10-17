using Microsoft.VisualStudio.TestTools.UnitTesting;
using redis.logic;

namespace UnitTests
{
    [TestClass]
    public class LogicTests
    {
        [TestMethod]
        public void CreateNewInstance()
        {
            var manager = new Manager();
            var instanceId = "testInstance";
            var instance = manager.CreateInstance(instanceId);
            Assert.IsNotNull(instance);
            Assert.AreEqual(instanceId, instance.instanceId);
            Assert.AreEqual("redpolo", instance.password);
            Assert.AreEqual(2001, instance.port);
            Assert.AreNotEqual(0, instance.processId);
            Assert.AreEqual("redis://" + instanceId + ":redpolo@hashtagredis.cloudapp.net:2001/", instance.connectionString);
        }

        [TestMethod]
        public void ReCreateInstance()
        {
            var manager = new Manager();
            var instanceId = "testInstance";
            var instanceA = manager.CreateInstance(instanceId);
            var instanceB = manager.CreateInstance(instanceId);
            Assert.IsNotNull(instanceB);
            Assert.AreEqual(instanceId, instanceB.instanceId);
            Assert.AreEqual("redpolo", instanceB.password);
            Assert.AreEqual(2001, instanceB.port);
            Assert.AreNotEqual(0, instanceB.processId);
            Assert.AreEqual("redis://" + instanceId + ":redpolo@hashtagredis.cloudapp.net:2001/", instanceB.connectionString);
        }

        [TestMethod]
        public void DeleteInstance()
        {
            var manager = new Manager();
            var instanceId = "testInstance";
            manager.CreateInstance(instanceId);
            manager.DeleteInstance(instanceId);
        }

        [TestMethod]
        public void IsInstanceRunning()
        {

        }
    }
}
