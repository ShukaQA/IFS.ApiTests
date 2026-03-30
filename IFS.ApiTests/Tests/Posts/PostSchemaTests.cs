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
    [AllureFeature("Schema Validation")]
    public class PostSchemaTests : BaseTest
    {
        [Test]
        [Category("schema")]
        [AllureTag("schema")]
        [AllureSeverity(SeverityLevel.critical)]
        [AllureDescription("Verify single post response matches expected JSON schema")]
        public void GetPostById_ResponseSchema_ShouldBeValid()
        {
            var response = ApiClient.Get<Post>("/posts/1");

            response.StatusCode.Should().Be(HttpStatusCode.OK);
            response.Content.Should().NotBeNullOrEmpty();
            SchemaValidator.Validate(response.Content!, "PostSchema.json");
        }

        [Test]
        [Category("schema")]
        [AllureTag("schema")]
        [AllureSeverity(SeverityLevel.normal)]
        [AllureDescription("Verify all posts response matches expected JSON schema")]
        public void GetAllPosts_ResponseSchema_ShouldBeValid()
        {
            var response = ApiClient.Get<List<Post>>("/posts");

            response.StatusCode.Should().Be(HttpStatusCode.OK);
            response.Content.Should().NotBeNullOrEmpty();
            SchemaValidator.Validate(response.Content!, "PostSchema.json");
        }
    }
}