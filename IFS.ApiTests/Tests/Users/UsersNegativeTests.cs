using Allure.Net.Commons;
using Allure.NUnit.Attributes;
using FluentAssertions;
using IFS.ApiTests.Helpers;
using IFS.ApiTests.Models;
using NUnit.Framework;
using System.Collections.Generic;
using System.Net;

namespace IFS.ApiTests.Tests.Users
{
    [TestFixture]
    [AllureSuite("Users API")]
    [AllureFeature("Negative Tests")]
    public class UsersNegativeTests : BaseTest
    {
        [TestCaseSource(typeof(TestDataLoader.Users), nameof(TestDataLoader.Users.InvalidUserIds))]
        [AllureTag("negative")]
        [AllureSeverity(SeverityLevel.normal)]
        [AllureDescription("Verify invalid user IDs return 404")]
        public void GetUserById_InvalidId_ShouldReturn404(int userId)
        {
            var response = ApiClient.Get<User>($"/users/{userId}");

            response.StatusCode.Should().Be(HttpStatusCode.NotFound,
                $"GET /users/{userId} should return 404 for invalid user");
        }

        [TestCaseSource(typeof(TestDataLoader.Users), nameof(TestDataLoader.Users.InvalidUserIds))]
        [AllureTag("negative")]
        [AllureSeverity(SeverityLevel.normal)]
        [AllureDescription("Verify non-existent user ID returns 404")]
        public void GetUserById_NonExistentId_ShouldReturn404(int userId)
        {
            var response = ApiClient.Get<User>($"/users/{userId}");

            response.StatusCode.Should().Be(HttpStatusCode.NotFound,
                "non-existent user should return 404");
        }

    }
}