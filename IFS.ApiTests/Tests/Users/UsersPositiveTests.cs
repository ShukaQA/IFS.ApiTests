using Allure.Net.Commons;
using Allure.NUnit.Attributes;
using FluentAssertions;
using IFS.ApiTests.Helpers;
using IFS.ApiTests.Models;
using NUnit.Framework;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net;

namespace IFS.ApiTests.Tests.Users
{
    [TestFixture]
    [AllureSuite("Users API")]
    [AllureFeature("Positive Tests")]
    public class UsersPositiveTests : BaseTest
    {
        [Test]
        [AllureTag("smoke")]
        [AllureSeverity(SeverityLevel.critical)]
        [AllureDescription("Verify GET /users returns 200 OK")]
        public void GetAllUsers_ShouldReturn200OK()
        {
            var response = ApiClient.Get<List<User>>("/users");

            response.StatusCode.Should().Be(HttpStatusCode.OK,
                "GET /users should return 200 OK");
        }

        [Test]
        [AllureTag("regression")]
        [AllureSeverity(SeverityLevel.normal)]
        [AllureDescription("Verify GET /users returns exactly 10 users")]
        public void GetAllUsers_ShouldReturn10Users()
        {
            var response = ApiClient.Get<List<User>>("/users");

            response.Data.Should().NotBeNull();
            response.Data.Should().HaveCount(10,
                "the API should return exactly 10 users");
        }

        [Test]
        [AllureTag("regression")]
        [AllureSeverity(SeverityLevel.normal)]
        [AllureDescription("Verify each user has required fields")]
        public void GetAllUsers_EachUser_ShouldHaveRequiredFields()
        {
            var response = ApiClient.Get<List<User>>("/users");

            response.Data.Should().NotBeNull();
            if (response.Data == null) Assert.Fail("Response data is null");

            foreach (var user in response.Data!)
            {
                user.Id.Should().BeGreaterThan(0, "each user must have a valid Id");
                user.Name.Should().NotBeNullOrWhiteSpace("each user must have a Name");
                user.Username.Should().NotBeNullOrWhiteSpace("each user must have a Username");
                user.Email.Should().NotBeNullOrWhiteSpace("each user must have an Email");
            }
        }

        [TestCaseSource(typeof(TestDataLoader.Users), nameof(TestDataLoader.Users.ValidUserIds))]
        [AllureTag("regression")]
        [AllureSeverity(SeverityLevel.critical)]
        [AllureDescription("Verify valid user IDs return correct user")]
        public void GetUserById_ValidId_ShouldReturnCorrectUser(int userId)
        {
            var response = ApiClient.Get<User>($"/users/{userId}");

            response.StatusCode.Should().Be(HttpStatusCode.OK,
                $"GET /users/{userId} should return 200 OK");
            response.Data.Should().NotBeNull();
            response.Data!.Id.Should().Be(userId,
                $"response should return user with Id {userId}");
        }

        [Test]
        [AllureTag("performance")]
        [AllureSeverity(SeverityLevel.minor)]
        [AllureDescription("Verify GET /users responds within time limit")]
        public void GetAllUsers_ShouldRespondWithinTimeLimit()
        {
            var stopwatch = Stopwatch.StartNew();
            var response = ApiClient.Get<List<User>>("/users");
            stopwatch.Stop();

            response.StatusCode.Should().Be(HttpStatusCode.OK);
            stopwatch.ElapsedMilliseconds.Should().BeLessThan(MaxResponseTimeMs,
                $"response time should be under {MaxResponseTimeMs}ms");
        }

        [Test]
        [AllureTag("regression")]
        [AllureSeverity(SeverityLevel.minor)]
        [AllureDescription("Verify GET /users/1/posts returns posts for user")]
        public void GetUserPosts_ShouldReturnPostsForUser()
        {
            var response = ApiClient.Get<List<dynamic>>("/users/1/posts");

            response.StatusCode.Should().Be(HttpStatusCode.OK,
                "GET /users/1/posts should return 200 OK");
            response.Data.Should().NotBeNull();
            response.Data!.Should().NotBeEmpty(
                "user 1 should have at least one post");
        }


    }

}