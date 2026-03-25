using Allure.NUnit.Attributes;
using Allure.Net.Commons;
using FluentAssertions;
using IFS.ApiTests.Helpers;
using IFS.ApiTests.Models;
using NUnit.Framework;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net;

namespace IFS.ApiTests.Tests
{
    [TestFixture]
    [AllureSuite("Posts API")]
    public class PostsTests : BaseTest
    {
        private const int MaxResponseTimeMs = 3000;

        // ── GET /posts ──────────────────────────────────────────────

        [Test]
        [AllureTag("smoke")]
        [AllureSeverity(SeverityLevel.critical)]
        [AllureFeature("GET /posts")]
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
        [AllureFeature("GET /posts")]
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
        [AllureFeature("GET /posts")]
        [AllureDescription("Verify each post has id, userId, title and body fields")]
        public void GetAllPosts_EachPost_ShouldHaveRequiredFields()
        {
            var response = ApiClient.Get<List<Post>>("/posts");

            response.Data.Should().NotBeNull();
            foreach (var post in response.Data)
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
        [AllureFeature("GET /posts")]
        [AllureDescription("Verify GET /posts responds within 3 seconds")]
        public void GetAllPosts_ShouldRespondWithinTimeLimit()
        {
            var stopwatch = Stopwatch.StartNew();
            var response = ApiClient.Get<List<Post>>("/posts");
            stopwatch.Stop();

            response.StatusCode.Should().Be(HttpStatusCode.OK);
            stopwatch.ElapsedMilliseconds.Should().BeLessThan(MaxResponseTimeMs,
                $"response time should be under {MaxResponseTimeMs}ms");
        }

        // ── GET /posts/{id} ─────────────────────────────────────────

        [TestCase(1)]
        [TestCase(50)]
        [TestCase(100)]
        [AllureTag("regression")]
        [AllureSeverity(SeverityLevel.critical)]
        [AllureFeature("GET /posts/{id}")]
        [AllureDescription("Verify valid post IDs return 200 OK")]
        public void GetPostById_ValidId_ShouldReturn200(int postId)
        {
            var response = ApiClient.Get<Post>($"/posts/{postId}");

            response.StatusCode.Should().Be(HttpStatusCode.OK,
                $"GET /posts/{postId} should return 200 OK");
            response.Data.Should().NotBeNull();
            response.Data.Id.Should().Be(postId,
                $"response should return post with Id {postId}");
        }

        [TestCase(0)]
        [TestCase(-1)]
        [TestCase(99999)]
        [AllureTag("regression")]
        [AllureSeverity(SeverityLevel.normal)]
        [AllureFeature("GET /posts/{id}")]
        [AllureDescription("Verify invalid post IDs return 404 Not Found")]
        public void GetPostById_InvalidId_ShouldReturn404(int postId)
        {
            var response = ApiClient.Get<Post>($"/posts/{postId}");

            response.StatusCode.Should().Be(HttpStatusCode.NotFound,
                $"GET /posts/{postId} should return 404 for non-existent post");
        }

        // ── POST /posts ─────────────────────────────────────────────

        [Test]
        [AllureTag("smoke")]
        [AllureSeverity(SeverityLevel.critical)]
        [AllureFeature("POST /posts")]
        [AllureDescription("Verify creating a post returns 201 Created")]
        public void CreatePost_ShouldReturn201Created()
        {
            var newPost = new Post { UserId = 1, Title = "Test Post", Body = "Test Body" };

            var response = ApiClient.Post<Post>("/posts", newPost);

            response.StatusCode.Should().Be(HttpStatusCode.Created,
                "creating a post should return 201 Created");
        }

        [Test]
        [AllureTag("regression")]
        [AllureSeverity(SeverityLevel.normal)]
        [AllureFeature("POST /posts")]
        [AllureDescription("Verify response body contains submitted data")]
        public void CreatePost_ResponseBody_ShouldContainSubmittedData()
        {
            var newPost = new Post { UserId = 1, Title = "My Title", Body = "My Body" };

            var response = ApiClient.Post<Post>("/posts", newPost);

            response.Data.Should().NotBeNull();
            response.Data.Title.Should().Be(newPost.Title,
                "response should echo back the submitted title");
            response.Data.Body.Should().Be(newPost.Body,
                "response should echo back the submitted body");
            response.Data.UserId.Should().Be(newPost.UserId,
                "response should echo back the submitted userId");
            response.Data.Id.Should().BeGreaterThan(0,
                "created post should have an assigned Id");
        }

        // ── PUT /posts/{id} ─────────────────────────────────────────

        [Test]
        [AllureTag("regression")]
        [AllureSeverity(SeverityLevel.normal)]
        [AllureFeature("PUT /posts/{id}")]
        [AllureDescription("Verify updating a post returns 200 and reflects changes")]
        public void UpdatePost_ShouldReturn200AndReflectChanges()
        {
            var updatedPost = new Post
            {
                Id = 1,
                UserId = 1,
                Title = "Updated Title",
                Body = "Updated Body"
            };

            var response = ApiClient.Put<Post>("/posts/1", updatedPost);

            response.StatusCode.Should().Be(HttpStatusCode.OK,
                "PUT /posts/1 should return 200 OK");
            response.Data.Title.Should().Be(updatedPost.Title,
                "response should reflect updated title");
            response.Data.Body.Should().Be(updatedPost.Body,
                "response should reflect updated body");
        }

        // ── DELETE /posts/{id} ──────────────────────────────────────

        [Test]
        [AllureTag("smoke")]
        [AllureSeverity(SeverityLevel.critical)]
        [AllureFeature("DELETE /posts/{id}")]
        [AllureDescription("Verify deleting a post returns 200 OK")]
        public void DeletePost_ShouldReturn200OK()
        {
            var response = ApiClient.Delete("/posts/1");

            response.StatusCode.Should().Be(HttpStatusCode.OK,
                "DELETE /posts/1 should return 200 OK");
        }

        // ── NESTED RESOURCE ─────────────────────────────────────────

        [Test]
        [AllureTag("regression")]
        [AllureSeverity(SeverityLevel.minor)]
        [AllureFeature("Nested Resources")]
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