using FluentAssertions;
using IFS.ApiTests.Helpers;
using IFS.ApiTests.Models;
using NUnit.Framework;
using System.Collections.Generic;
using System.Net;

namespace IFS.ApiTests.Tests
{
    [TestFixture]
    public class PostsTests : BaseTest
    {
        // ── GET /posts ──────────────────────────────────────────────
        [Test]
        public void GetAllPosts_ShouldReturn200OK()
        {
            var response = ApiClient.Get<List<Post>>("/posts");

            response.StatusCode
                .Should().Be(HttpStatusCode.OK, "GET /posts should return 200 OK");
        }

        [Test]
        public void GetAllPosts_ShouldReturn100Posts()
        {
            var response = ApiClient.Get<List<Post>>("/posts");

            response.Data.Should().NotBeNull();
            response.Data.Should().HaveCount(100, "the API should return exactly 100 posts");
        }

        [Test]
        public void GetAllPosts_EachPost_ShouldHaveRequiredFields()
        {
            var response = ApiClient.Get<List<Post>>("/posts");

            response.Data.Should().NotBeNull();
            foreach (var post in response.Data)
            {
                post.Id.Should().BeGreaterThan(0, "each post should have a valid Id");
                post.UserId.Should().BeGreaterThan(0, "each post should have a valid UserId");
                post.Title.Should().NotBeNullOrWhiteSpace("each post should have a Title");
                post.Body.Should().NotBeNullOrWhiteSpace("each post should have a Body");
            }
        }

        // ── GET /posts/{id} ─────────────────────────────────────────
        [Test]
        public void GetPostById_ValidId_ShouldReturnCorrectPost()
        {
            var response = ApiClient.Get<Post>("/posts/1");

            response.StatusCode.Should().Be(HttpStatusCode.OK);
            response.Data.Should().NotBeNull();
            response.Data.Id.Should().Be(1, "should return the post with Id 1");
        }

        [Test]
        public void GetPostById_NonExistentId_ShouldReturn404()
        {
            var response = ApiClient.Get<Post>("/posts/99999");

            response.StatusCode
                .Should().Be(HttpStatusCode.NotFound, "non-existent post should return 404");
        }

        // ── POST /posts ─────────────────────────────────────────────
        [Test]
        public void CreatePost_ShouldReturn201Created()
        {
            var newPost = new Post { UserId = 1, Title = "Test Post", Body = "Test Body" };

            var response = ApiClient.Post<Post>("/posts", newPost);

            response.StatusCode
                .Should().Be(HttpStatusCode.Created, "creating a post should return 201 Created");
        }

        [Test]
        public void CreatePost_ResponseBody_ShouldContainSubmittedData()
        {
            var newPost = new Post { UserId = 1, Title = "My Title", Body = "My Body" };

            var response = ApiClient.Post<Post>("/posts", newPost);

            response.Data.Should().NotBeNull();
            response.Data.Title.Should().Be(newPost.Title, "response should echo back the submitted title");
            response.Data.Body.Should().Be(newPost.Body, "response should echo back the submitted body");
            response.Data.UserId.Should().Be(newPost.UserId, "response should echo back the submitted userId");
        }

        // ── PUT /posts/{id} ─────────────────────────────────────────
        [Test]
        public void UpdatePost_ShouldReturn200AndUpdatedData()
        {
            var updatedPost = new Post { Id = 1, UserId = 1, Title = "Updated Title", Body = "Updated Body" };

            var response = ApiClient.Put<Post>("/posts/1", updatedPost);

            response.StatusCode.Should().Be(HttpStatusCode.OK, "PUT /posts/1 should return 200 OK");
            response.Data.Title.Should().Be(updatedPost.Title, "title should reflect the update");
            response.Data.Body.Should().Be(updatedPost.Body, "body should reflect the update");
        }

        [Test]
        public void DeletePost_ShouldReturn200OK()
        {
            var response = ApiClient.Delete("/posts/1");

            response.StatusCode
                .Should().Be(HttpStatusCode.OK, "DELETE /posts/1 should return 200 OK");
        }
    }
}