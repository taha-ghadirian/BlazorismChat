using BlazorismChat.DbLayer.Entities.Users;
using Xunit;
using FluentAssertions;

namespace BlazorismChat.DbLayerTests
{
    public class UserTests
    {
        [Fact]
        public void TestFixedEmail()
        {
            //Given
            var user = new User();

            //When
            user.Email = "TestMail@outlook.com";

            //Then
            user.FixedEmail.Should().BeEquivalentTo(user.Email);
            user.FixedEmail.Should().Be("TESTMAIL@OUTLOOK.COM");
        }

        [Fact]
        public void TestFixedUserName()
        {
            //Given
            var user = new User();

            //When
            user.UserName = "TestUser";

            //Then
            user.FixedUserName.Should().BeEquivalentTo(user.UserName);
            user.FixedUserName.Should().Be("TESTUSER");
        }

        
    }
}