using HannerLabApp.Models;
using Newtonsoft.Json;
using NUnit.Framework;

namespace HannerLabApp.Tests
{
    public class Tests
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void Test1()
        {
            // Load testing export data from json in ./Data
            string json = System.IO.File.ReadAllText(@"Data/test_export_activity.json");
            Export export = JsonConvert.DeserializeObject<Export>(json);

            Assert.Pass();
        }
    }
}