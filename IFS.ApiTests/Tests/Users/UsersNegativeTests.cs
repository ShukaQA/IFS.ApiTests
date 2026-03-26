using Allure.Net.Commons;
using Allure.NUnit.Attributes;
using FluentAssertions;
using IFS.ApiTests.Helpers;
using IFS.ApiTests.Models;
using IFS.ApiTests.Tests.Posts;
using NUnit.Framework;
using System.Net;

namespace IFS.ApiTests.Tests.Users
{
    [TestFixture]
    [AllureSuite("Users API")]
    [AllureFeature("Negative Tests")]
    public class UsersNegativeTests : BaseTest
    {
        [Test]
        [AllureTag("negative")]
        [AllureSeverity(SeverityLevel.normal)]
        [AllureDescription("Verify non-existent user ID returns 404")]
        public void GetUserById_NonExistentId_ShouldReturn404()
        {
            var response = ApiClient.Get<User>("/users/99999");

            response.StatusCode.Should().Be(HttpStatusCode.NotFound,
                "non-existent user should return 404");
        }

        [TestCase(0)]
        [TestCase(-1)]
        [AllureTag("negative")]
        [AllureSeverity(SeverityLevel.normal)]
        [AllureDescription("Verify invalid user IDs return 404")]
        public void GetUserById_InvalidId_ShouldReturn404(int userId)
        {
            var response = ApiClient.Get<User>($"/users/{userId}");

            response.StatusCode.Should().Be(HttpStatusCode.NotFound,
                $"GET /users/{userId} should return 404 for invalid user");
        }

        [Test]
        [AllureTag("negative")]
        [AllureSeverity(SeverityLevel.minor)]
        [AllureDescription("Verify GET /users/99999/posts returns empty or 404")]
        public void GetUserPosts_NonExistentUser_ShouldReturnEmptyOrNotFound()
        {
            var response = ApiClient.Get<List<dynamic>>("/users/99999/posts");

            var isEmptyOrNotFound =
                response.StatusCode == HttpStatusCode.NotFound ||
                (response.StatusCode == HttpStatusCode.OK && (response.Data == null || response.Data.Count == 0));

            isEmptyOrNotFound.Should().BeTrue(
                "non-existent user should return 404 or empty posts list");
        }
    }
}