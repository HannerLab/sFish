using System;
using Dropbox.Api.Users;
using HannerLabApp.Configuration;
using HannerLabApp.Models;
using HannerLabApp.Services.Exporters;
using Newtonsoft.Json;
using NUnit.Framework;

namespace HannerLabApp.Tests
{
    /// <summary>
    /// TODO: Make tests
    /// </summary>
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