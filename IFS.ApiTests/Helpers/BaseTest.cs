using IFS.ApiTests.Clients;
using NUnit.Framework;

namespace IFS.ApiTests.Helpers
{
    public abstract class BaseTest
    {
        protected ApiClient ApiClient;

        [SetUp]
        public void Setup()
        {
            ApiClient = new ApiClient();
        }

        [TearDown]
        public void TearDown()
        {
            // Add logging or cleanup here if needed
        }
    }
}