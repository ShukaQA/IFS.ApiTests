using NUnit.Framework;

namespace IFS.ApiTests.Helpers
{
    [SetUpFixture]
    public class GlobalSetup
    {
        [OneTimeSetUp]
        public void ClearAllureResults()
        {
            var allureResultsPath = Path.Combine(
                AppContext.BaseDirectory, "allure-results");

            if (Directory.Exists(allureResultsPath))
            {
                Directory.Delete(allureResultsPath, recursive: true);
                Console.WriteLine($">>> Allure results cleared: {allureResultsPath}");
            }

            Directory.CreateDirectory(allureResultsPath);
            Console.WriteLine(">>> Allure results folder recreated fresh");
        }
    }
}