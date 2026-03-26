using Allure.Net.Commons;
using Allure.NUnit.Attributes;
using FluentAssertions;
using IFS.ApiTests.Helpers;
using IFS.ApiTests.Models;
using IFS.ApiTests.TestData;
using NUnit.Framework;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net;

namespace IFS.ApiTests.Tests.Posts
{
    [TestFixture]
    [AllureSuite("Posts API")]
    [AllureFeature("Positive Tests")]
    public class PostsPositiveTests : BaseTest
    {
        [Test]
        [AllureTag("smoke")]
        [AllureSeverity(SeverityLevel.critical)]
        [AllureDescription("Verify GET /posts returns 200 OK")]
        public void GetAllPosts_ShouldReturn200OK()
        {
            var response = ApiClient.Get<List<Post>>("/posts");

            response.StatusCode.Should().Be(HttpStatusCode.OK,
                "GET /posts should return 200 OK");
        }

        [Test]
        [AllureTag("regression")]
        [AllureSeverity(SeverityLevel.normal)]
        [AllureDescription("Verify GET /posts returns exactly 100 posts")]
        public void GetAllPosts_ShouldReturn100Posts()
        {
            var response = ApiClient.Get<List<Post>>("/posts");

            response.Data.Should().NotBeNull();
            response.Data.Should().HaveCount(100,
                "the API should return exactly 100 posts");
        }

        [Test]
        [AllureTag("regression")]
        [AllureSeverity(SeverityLevel.normal)]
        [AllureDescription("Verify each post has id, userId, title and body fields")]
        public void GetAllPosts_EachPost_ShouldHaveRequiredFields()
        {
            var response = ApiClient.Get<List<Post>>("/posts");

            response.Data.Should().NotBeNull();
            if (response.Data == null) Assert.Fail("Response data is null");

            foreach (var post in response.Data!)
            {
                post.Id.Should().BeGreaterThan(0, "each post must have a valid Id");
                post.UserId.Should().BeGreaterThan(0, "each post must have a valid UserId");
                post.Title.Should().NotBeNullOrWhiteSpace("each post must have a Title");
                post.Body.Should().NotBeNullOrWhiteSpace("each post must have a Body");
            }
        }

        [Test]
        [AllureTag("performance")]
        [AllureSeverity(SeverityLevel.minor)]
        [AllureDescription("Verify GET /posts responds within time limit")]
        public void GetAllPosts_ShouldRespondWithinTimeLimit()
        {
            var stopwatch = Stopwatch.StartNew();
            var response = ApiClient.Get<List<Post>>("/posts");
            stopwatch.Stop();

            response.StatusCode.Should().Be(HttpStatusCode.OK);
            stopwatch.ElapsedMilliseconds.Should().BeLessThan(MaxResponseTimeMs,
                $"response time should be under {MaxResponseTimeMs}ms");
        }

        [TestCaseSource(typeof(TestDataLoader.Posts), nameof(TestDataLoader.Posts.ValidPostIds))]
        [AllureTag("regression")]
        [AllureSeverity(SeverityLevel.critical)]
        [AllureDescription("Verify valid post IDs return 200 OK with correct data")]
        public void GetPostById_ValidId_ShouldReturn200WithCorrectData(int postId)
        {
            var response = ApiClient.Get<Post>($"/posts/{postId}");

            response.StatusCode.Should().Be(HttpStatusCode.OK,
                $"GET /posts/{postId} should return 200 OK");
            response.Data.Should().NotBeNull();
            response.Data!.Id.Should().Be(postId,
                $"response should return post with Id {postId}");
        }

        [Test]
        [AllureTag("smoke")]
        [AllureSeverity(SeverityLevel.critical)]
        [AllureDescription("Verify creating a post returns 201 Created")]
        public void CreatePost_ShouldReturn201Created()
        {
            var response = ApiClient.Post<Post>("/posts", TestDataLoader.Posts.ValidNewPost);

            response.StatusCode.Should().Be(HttpStatusCode.Created,
                "creating a post should return 201 Created");
        }

        [Test]
        [AllureTag("regression")]
        [AllureSeverity(SeverityLevel.normal)]
        [AllureDescription("Verify response body contains submitted data")]
        public void CreatePost_ResponseBody_ShouldContainSubmittedData()
        {
            var newPost = TestDataLoader.Posts.ValidNewPost;
            var response = ApiClient.Post<Post>("/posts", newPost);

            response.Data.Should().NotBeNull();
            response.Data!.Title.Should().Be(newPost.Title,
                "response should echo back the submitted title");
            response.Data!.Body.Should().Be(newPost.Body,
                "response should echo back the submitted body");
            response.Data!.UserId.Should().Be(newPost.UserId,
                "response should echo back the submitted userId");
            response.Data!.Id.Should().BeGreaterThan(0,
                "created post should have an assigned Id");
        }

        [Test]
        [AllureTag("regression")]
        [AllureSeverity(SeverityLevel.normal)]
        [AllureDescription("Verify updating a post returns 200 and reflects changes")]
        public void UpdatePost_ShouldReturn200AndReflectChanges()
        {
            var updatedPost = TestDataLoader.Posts.UpdatedPost;
            var response = ApiClient.Put<Post>("/posts/1", updatedPost);

            response.StatusCode.Should().Be(HttpStatusCode.OK,
                "PUT /posts/1 should return 200 OK");
            response.Data!.Title.Should().Be(updatedPost.Title,
                "response should reflect updated title");
            response.Data!.Body.Should().Be(updatedPost.Body,
                "response should reflect updated body");
        }

        [Test]
        [AllureTag("smoke")]
        [AllureSeverity(SeverityLevel.critical)]
        [AllureDescription("Verify deleting a post returns 200 OK")]
        public void DeletePost_ShouldReturn200OK()
        {
            var response = ApiClient.Delete("/posts/1");

            response.StatusCode.Should().Be(HttpStatusCode.OK,
                "DELETE /posts/1 should return 200 OK");
        }

        [Test]
        [AllureTag("regression")]
        [AllureSeverity(SeverityLevel.minor)]
        [AllureDescription("Verify GET /posts/1/comments returns comments")]
        public void GetPostComments_ShouldReturnCommentsForPost()
        {
            var response = ApiClient.Get<List<dynamic>>("/posts/1/comments");

            response.StatusCode.Should().Be(HttpStatusCode.OK,
                "GET /posts/1/comments should return 200 OK");
            response.Data.Should().NotBeNull();
            response.Data.Should().NotBeEmpty(
                "post 1 should have at least one comment");
        }
    }
}