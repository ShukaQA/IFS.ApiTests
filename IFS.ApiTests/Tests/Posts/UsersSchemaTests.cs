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
    [AllureFeature("Schema Validation")]
    public class UsersSchemaTests : BaseTest
    {
        [Test]
        [Category("schema")]
        [AllureTag("schema")]
        [AllureSeverity(SeverityLevel.critical)]
        [AllureDescription("Verify single user response matches expected JSON schema")]
        public void GetUserById_ResponseSchema_ShouldBeValid()
        {
            var response = ApiClient.Get<User>("/users/1");

            response.StatusCode.Should().Be(HttpStatusCode.OK);
            response.Content.Should().NotBeNullOrEmpty();
            SchemaValidator.Validate(response.Content!, "UserSchema.json");
        }

        [Test]
        [Category("schema")]
        [AllureTag("schema")]
        [AllureSeverity(SeverityLevel.normal)]
        [AllureDescription("Verify all users response matches expected JSON schema")]
        public void GetAllUsers_ResponseSchema_ShouldBeValid()
        {
            var response = ApiClient.Get<List<User>>("/users");

            response.StatusCode.Should().Be(HttpStatusCode.OK);
            response.Content.Should().NotBeNullOrEmpty();
            SchemaValidator.Validate(response.Content!, "UserSchema.json");
        }
    }
}