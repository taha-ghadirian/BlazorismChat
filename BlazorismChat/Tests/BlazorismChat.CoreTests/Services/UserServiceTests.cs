using System;
using BlazorismChat.Core.ServerServices.Interfaces;
using BlazorismChat.CoreTests.Helpers;
using BlazorismChat.DbLayer.DbContexts;
using Bunit;
using Xunit;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using BlazorismChat.DbLayer.Entities.Users;
using AutoFixture.Xunit2;
using AutoFixture;
using Microsoft.EntityFrameworkCore;
using BlazorismChat.Core.Security;
using BlazorismChat.ClientLibraries.DTOs;
using System.Linq;
using BlazorismChat.Core.ServerServices;


#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
#pragma warning disable CS8601 // Possible null reference assignment.
#pragma warning disable CS8604 // Possible null reference argument.

namespace BlazorismChat.CoreTests.Services;

public class UserServicesTests : TestContext
{
    private readonly TestDb _dbContext;
    private readonly IUserService _userService;

    public UserServicesTests()
    {
        this.AddBlazorismChatSupport();
        _dbContext = Services.GetService(typeof(TestDb)) as TestDb;
        _userService = new UserService(_dbContext);
        _dbContext.FirstInitDb();
    }

    #region Get User Tests

    [Fact]
    public async void GetAllUsers_Should_Return2_Users()
    {
        // Arrange
        var users = await _userService.GetAllUsers();

        // Asserts
        users.Should().HaveCount(2);
    }

    [Theory]
    [InlineData(1, true)]
    [InlineData(2, true)]
    [InlineData(4, false)]
    public async void GetUserById_Should_Return_User(int id, bool shouldReturn)
    {
        // Arrange
        var user = await _userService.GetUserById(id);

        // Assert
        if (shouldReturn)
            user.Should().NotBeNull().And.BeOfType<User>();
        else
            user.Should().BeNull();
    }

    [Theory]
    [InlineData("tESt1", true)]
    [InlineData("test2", true)]
    [InlineData("test3", false)]
    public async void GetUserByName_Should_Return_User(string name, bool shouldReturn)
    {
        // Arrange
        var user = await _userService.GetUserByUsername(name);

        // Assert
        if (shouldReturn)
            user.Should().NotBeNull().And.BeOfType<User>();
        else
            user.Should().BeNull();
    }

    [Theory]
    [InlineData("tESt1@gmail.com", true)]
    [InlineData("test2@gmail.com", true)]
    [InlineData("test3@gmail.com", false)]
    public async void GetUserByEmail_Should_Return_User(string email, bool shouldReturn)
    {
        // Arrange
        var user = await _userService.GetUserByEmail(email);

        // Assert
        if (shouldReturn)
            user.Should().NotBeNull().And.BeOfType<User>();
        else
            user.Should().BeNull();
    }

    #endregion

    #region Create User Tests

    [Theory, UserAutoData]
    public async void CreateUser_Should_Return_User(User user)
    {
        // Arrange
        var createdUser = await _userService.CreateUser(user);
        var userFromDb = await _userService.GetUserById(createdUser.UserId);

        var count = _dbContext.Users.Count();

        // Assert
        createdUser.Should().NotBeNull().And.BeOfType<User>().And.BeEquivalentTo(user);
        createdUser.UserId.Should().BeGreaterThan(0);
        userFromDb.Should().NotBeNull().And.BeOfType<User>().And.BeEquivalentTo(user);
        count.Should().Be(3);

        // Cleanup
        await _userService.DeleteUser(createdUser, true);
    }

    [Theory, UserAutoData]
    public async void CreateUser_Should_BeNull_When_User_Exists(User user)
    {
        // Arrange
        user.UserName = "test1";
        var createdUser = await _userService.CreateUser(user);

        // Assert
        createdUser.Should().BeNull();

        // Cleanup
        await _userService.DeleteUser(createdUser, true);

        //------------------

        // Arrange
        user.Email = "test1@gmail.com";
        user.UserName = "Random User";
        createdUser = await _userService.CreateUser(user);

        // Assert
        createdUser.Should().BeNull();

        // Cleanup
        await _userService.DeleteUser(createdUser, true);
    }

    #endregion

    #region Update Tests

    [Theory, UserAutoData]
    public async void UpdateUser_Should_Return_User(User user)
    {
        // Arrange
        var createdUser = await _userService.CreateUser(user);

        // Act
        createdUser.FirstName = "updated";
        var updatedUser = await _userService.UpdateUser(createdUser);
        var userFromDb = await _userService.GetUserById(createdUser.UserId);
        var count = _dbContext.Users.Count();

        // Assert
        createdUser.UserId.Should().BeGreaterThan(0).And.Be(updatedUser.UserId).And.Be(userFromDb.UserId);
        updatedUser.Should().NotBeNull().And.BeOfType<User>().And.BeEquivalentTo(createdUser);
        userFromDb.Should().NotBeNull().And.BeOfType<User>().And.BeEquivalentTo(createdUser);
        count.Should().Be(3);

        // Cleanup
        await _userService.DeleteUser(createdUser, true);
    }

    [Theory, UserAutoData]
    public async void UpdateUser_WithSecurityKeyChange_Should_Return_User(User user)
    {
        // Arrange
        var createdUser = await _userService.CreateUser(user);
        var currentKey = createdUser.ActiveCode;

        // Act
        createdUser.FirstName = "updated";
        var updatedUser = await _userService.UpdateUser(createdUser, true);
        var userFromDb = await _userService.GetUserById(createdUser.UserId);
        var count = _dbContext.Users.Count();

        // Assert
        createdUser.UserId.Should().BeGreaterThan(0).And.Be(updatedUser.UserId).And.Be(userFromDb.UserId);
        updatedUser.Should().NotBeNull().And.BeOfType<User>().And.BeEquivalentTo(createdUser);
        userFromDb.Should().NotBeNull().And.BeOfType<User>().And.BeEquivalentTo(createdUser);
        count.Should().Be(3);
        updatedUser.ActiveCode.Should().NotBeEquivalentTo(currentKey);

        // Cleanup
        await _userService.DeleteUser(createdUser, true);
    }

    #endregion

    #region Exists check


    [Theory]
    [InlineData(1, true)]
    [InlineData(2, true)]
    [InlineData(4, false)]
    public async void UserExists_Should_Return_Bool(int id, bool shouldReturn)
    {
        // Arrange
        var exists = await _userService.IsUserExists(id);

        // Assert
        exists.Should().Be(shouldReturn);
    }

    [Theory]
    [InlineData("tESt1", true)]
    [InlineData("test2", true)]
    [InlineData("test3", false)]
    public async void UserNameExists_Should_Return_Bool(string name, bool shouldReturn)
    {
        // Arrange
        var exists = await _userService.IsUsernameExists(name);

        // Assert
        exists.Should().Be(shouldReturn);
    }

    [Theory]
    [InlineData("tESt1@gmail.com", true)]
    [InlineData("Test2@gmail.com", true)]
    [InlineData("test3@gmail.com", false)]
    public async void EmailExists_Should_Return_Bool(string email, bool shouldReturn)
    {
        // Arrange
        var exists = await _userService.IsEmailExists(email);

        // Assert
        exists.Should().Be(shouldReturn);
    }

    #endregion

    #region Delete Tests

    [Theory, UserAutoData]
    public async void DeleteUser_WithoutFullyDelete_Should_Return_True(User user)
    {
        // Arrange
        var createdUser = await _userService.CreateUser(user);
        var count = _dbContext.Users.Count();

        // Act
        var deleted = await _userService.DeleteUser(createdUser, false);
        var userFromDb = await _userService.GetUserById(createdUser.UserId);
        var countAfter = _dbContext.Users.Count();
        var realCount = _dbContext.Users.IgnoreQueryFilters().Count();
        var realUser = _dbContext.Users.IgnoreQueryFilters().FirstOrDefault(u => u.UserId == createdUser.UserId);

        // Assert
        deleted.Should().BeTrue();
        userFromDb.Should().BeNull();
        countAfter.Should().Be(count - 1);
        realCount.Should().Be(count);
        realUser.Should().NotBeNull();
        realUser.IsDeleted.Should().BeTrue();

        // Cleanup
        await _userService.DeleteUser(createdUser, true);
    }

    [Theory, UserAutoData]
    public async void DeleteUser_WithFullyDelete_Should_Return_True(User user)
    {
        // Arrange
        var createdUser = await _userService.CreateUser(user);
        var count = _dbContext.Users.Count();

        // Act
        var deleted = await _userService.DeleteUser(createdUser, true);
        var userFromDb = await _userService.GetUserById(createdUser.UserId);
        var countAfter = _dbContext.Users.Count();
        var realCount = _dbContext.Users.IgnoreQueryFilters().Count();
        var realUser = _dbContext.Users.IgnoreQueryFilters().FirstOrDefault(u => u.UserId == createdUser.UserId);

        // Assert
        deleted.Should().BeTrue();
        userFromDb.Should().BeNull();
        countAfter.Should().Be(count - 1);
        realCount.Should().Be(count - 1);
        realUser.Should().BeNull();

        // Cleanup
        await _userService.DeleteUser(createdUser, true);
    }

    [Theory, UserAutoData]
    public async void DeleteUser_WithUnknownUser_Should_Return_False(User user)
    {
        // Act
        var deleted = await _userService.DeleteUser(user, true);

        // Assert
        deleted.Should().BeFalse();

        //-------------------

        // Act
        deleted = await _userService.DeleteUser(user, false);

        // Assert
        deleted.Should().BeFalse();
    }

    [Theory]
    [InlineData(1, true)]
    [InlineData(4, false)]
    public async void DeleteUser_WithId_Should_Return_Bool(int id, bool shouldReturn)
    {
        // Act
        var deleted = await _userService.DeleteUser(id, true);
        var userFromDb = await _userService.GetUserById(id);

        // Assert
        deleted.Should().Be(shouldReturn);
        userFromDb.Should().BeNull();

        //-------------------

        // Act
        deleted = await _userService.DeleteUser(id + 1, false);
        userFromDb = await _userService.GetUserById(id + 1);

        // Assert
        deleted.Should().Be(shouldReturn);
        userFromDb.Should().BeNull();
    }

    #endregion

    #region Password Update Tests

    [Theory, UserAutoData]
    public async void UpdatePassword_Should_Return_True(User user)
    {
        // Arrange
        var createdUser = await _userService.CreateUser(user);
        var identityCode = createdUser.IdentityCode;
        var activeCode = createdUser.ActiveCode;
        var currentPassword = createdUser.Password;

        // Act
        var updated = await _userService.UpdateUserPassword(createdUser, "newPassword");
        var userFromDb = await _userService.GetUserById(createdUser.UserId);

        // Assert
        updated.Should().BeTrue();
        userFromDb.Password.Should().NotBe(currentPassword).And.NotBe(PasswordHelper.EncodePasswordMd5(currentPassword));
        userFromDb.IdentityCode.Should().NotBeEquivalentTo(identityCode);
        userFromDb.ActiveCode.Should().NotBeEquivalentTo(activeCode);

        // Clean up
        await _userService.DeleteUser(createdUser, true);
    }

    [Theory, UserAutoData]
    public async void UpdatePassword_WithUnknownUser_Should_Return_False(User user)
    {
        // Act
        var updated = await _userService.UpdateUserPassword(user, "newPassword");

        // Assert
        updated.Should().BeFalse();
    }

    [Theory, UserAutoData]
    public async void UpdatePassword_WithNullPassword_Should_Return_False(User user)
    {
        // Arrange
        var createdUser = await _userService.CreateUser(user);
        var currentPassword = createdUser.Password;

        // Act
        var updated = await _userService.UpdateUserPassword(createdUser, null);
        var userFromDb = await _userService.GetUserById(createdUser.UserId);

        // Assert
        updated.Should().BeFalse();
        userFromDb.Password.Should().Be(currentPassword);

        // CleanUp
        await _userService.DeleteUser(createdUser, true);
    }

    [Theory, UserAutoData]
    public async void UpdatePassword_WithEmptyPassword_Should_Return_False(User user)
    {
        // Arrange
        var createdUser = await _userService.CreateUser(user);
        var currentPassword = createdUser.Password;

        // Act
        var updated = await _userService.UpdateUserPassword(createdUser, string.Empty);
        var userFromDb = await _userService.GetUserById(createdUser.UserId);

        // Assert
        updated.Should().BeFalse();
        userFromDb.Password.Should().Be(currentPassword);

        // Cleanup
        await _userService.DeleteUser(createdUser, true);
    }

    [Fact]
    public async void UpdatePassword_WithEmptyUser_Should_Return_False()
    {
        // Act
        var updated = await _userService.UpdateUserPassword(new User(), "newPassword");

        // Assert
        updated.Should().BeFalse();
    }

    #endregion

    # region Login Tests

    [Theory, RegisterDTOAutoData]
    public async void Login_WithValidUser_Should_Return_True(RegisterDTO user)
    {
        // Arrange
        var password = "pass";
        user.Password = password;
        var createdUser = await _userService.RegisterUser(user);

        // Act
        var loggedIn = await _userService.LoginUser(user.UserName, password);

        // Assert
        loggedIn.Should().NotBeNull().And.BeOfType<User>().And.BeEquivalentTo(createdUser);

        // Cleanup
        await _userService.DeleteUser(createdUser, true);
    }

    [Theory, RegisterDTOAutoData]
    public async void Login_WithInvalidPassword_Should_Return_False(RegisterDTO user)
    {
        // Arrange
        var password = "pass";
        user.Password = password;
        var createdUser = await _userService.RegisterUser(user);

        // Act
        var loggedIn = await _userService.LoginUser(user.UserName, password + "1");

        // Assert
        loggedIn.Should().BeNull();

        // Cleanup
        await _userService.DeleteUser(createdUser, true);
    }

    [Theory, RegisterDTOAutoData]
    public async void Login_WithInvalidUserName_Should_Return_False(RegisterDTO user)
    {
        // Arrange
        var password = "pass";
        user.Password = password;
        var createdUser = await _userService.RegisterUser(user);

        // Act
        var loggedIn = await _userService.LoginUser(user.UserName + "1", password);

        // Assert
        loggedIn.Should().BeNull();

        // Cleanup
        await _userService.DeleteUser(createdUser, true);
    }

    [Fact]
    public async void Login_WithEmptyUser_Should_Return_False()
    {
        // Act
        var loggedIn = await _userService.LoginUser(string.Empty, "pass");

        // Assert
        loggedIn.Should().BeNull();
    }

    [Fact]
    public async void Login_WithEmptyPassword_Should_Return_False()
    {
        // Act
        var loggedIn = await _userService.LoginUser("user", string.Empty);

        // Assert
        loggedIn.Should().BeNull();
    }

    #endregion

    #region Register Tests

    [Theory, RegisterDTOAutoData]
    public async void Register_WithValidUser_Should_Return_True(RegisterDTO user)
    {
        // Arrange
        var createdUser = await _userService.RegisterUser(user);

        // Act
        var registered = await _userService.IsUsernameExists(user.UserName);

        // Assert
        createdUser.Should().NotBeNull().And.BeOfType<User>();
        registered.Should().BeTrue();

        // Cleanup
        await _userService.DeleteUser(createdUser, true);
    }

    [Theory, RegisterDTOAutoData]
    public async void Register_WithInvalidUser_Should_Return_False(RegisterDTO user)
    {
        // Arrange
        var username = "test1";
        user.UserName = username;
        var createdUser = await _userService.RegisterUser(user);

        // Assert
        createdUser.Should().BeNull();

        // Cleanup
        await _userService.DeleteUser(createdUser, true);

        //-------------------------

        // Arrange
        var email = "test1@gmail.com";
        user.UserName = "RANDOM USER";
        user.Email = email;
        createdUser = await _userService.RegisterUser(user);

        // Assert
        createdUser.Should().BeNull();

        // Cleanup
        await _userService.DeleteUser(createdUser, true);
    }

    [Fact]
    public async void Register_WithEmptyUser_Should_Return_False()
    {
        // Act
        var registered = await _userService.RegisterUser(new RegisterDTO());

        // Assert
        registered.Should().BeNull();

        // Cleanup
        await _userService.DeleteUser(registered, true);
    }

    #endregion
}

#pragma warning restore CS8601 // Possible null reference assignment.
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
#pragma warning restore CS8604 // Possible null reference argument.