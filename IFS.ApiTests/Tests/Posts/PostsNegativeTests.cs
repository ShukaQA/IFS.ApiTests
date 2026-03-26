using Allure.Net.Commons;
using Allure.NUnit.Attributes;
using FluentAssertions;
using IFS.ApiTests.Helpers;
using IFS.ApiTests.Models;
using NUnit.Framework;
using System.Collections.Generic;
using System.Net;

namespace IFS.ApiTests.Tests.Posts
{
    [TestFixture]
    [AllureSuite("Posts API")]
    [AllureFeature("Negative Tests")]
    public class PostsNegativeTests : BaseTest
    {
        [TestCaseSource(typeof(TestDataLoader.Posts), nameof(TestDataLoader.Posts.InvalidPostIds))]
        [AllureTag("negative")]
        [AllureSeverity(SeverityLevel.normal)]
        [AllureDescription("Verify invalid post IDs return 404 Not Found")]
        public void GetPostById_InvalidId_ShouldReturn404(int postId)
        {
            var response = ApiClient.Get<Post>($"/posts/{postId}");

            response.StatusCode.Should().Be(HttpStatusCode.NotFound,
                $"GET /posts/{postId} should return 404 for non-existent post");
        }

        [Test]
        [AllureTag("negative")]
        [AllureSeverity(SeverityLevel.normal)]
        [AllureDescription("Verify creating a post with empty title still returns 201")]
        public void CreatePost_WithEmptyTitle_ShouldStillReturn201()
        {
            var response = ApiClient.Post<Post>("/posts", TestDataLoader.Posts.EmptyTitlePost);

            response.StatusCode.Should().Be(HttpStatusCode.Created,
                "JSONPlaceholder accepts empty title — fake API does not validate");
        }

        [Test]
        [AllureTag("negative")]
        [AllureSeverity(SeverityLevel.minor)]
        [AllureDescription("Verify GET /posts/99999/comments returns empty or 404")]
        public void GetPostComments_NonExistentPost_ShouldReturnEmptyOrNotFound()
        {
            var response = ApiClient.Get<List<dynamic>>("/posts/99999/comments");

            var isEmptyOrNotFound =
                response.StatusCode == HttpStatusCode.NotFound ||
                (response.StatusCode == HttpStatusCode.OK &&
                (response.Data == null || response.Data.Count == 0));

            isEmptyOrNotFound.Should().BeTrue(
                "non-existent post should return 404 or empty comments list");
        }
    }
}