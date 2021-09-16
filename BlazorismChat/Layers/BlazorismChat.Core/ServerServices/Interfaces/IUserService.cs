using BlazorismChat.ClientLibraries.DTOs;
using BlazorismChat.DbLayer.Entities.Users;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BlazorismChat.Core.ServerServices.Interfaces;

public interface IUserService
{
    Task<bool> IsUsernameExists(string username);
    Task<bool> IsEmailExists(string email);
    Task<bool> IsUserExists(int userId);

    Task<User?> GetUserByUsername(string username);
    Task<User?> GetUserByEmail(string email);
    Task<User?> GetUserById(int userId);

    Task<User?> CreateUser(User user);
    Task<User?> UpdateUser(User user, bool updateSecurityKey = false);
    Task<bool> DeleteUser(User user, bool fullyDelete = false);
    Task<bool> DeleteUser(int userId, bool fullyDelete = false);

    Task<bool> UpdateUserPassword(User user, string password);
    Task<bool> UpdateUserPassword(string username, string password);

    Task<User?> RegisterUser(RegisterDTO registerDTO);
    Task<User?> LoginUser(string username, string password);

    Task<List<User>> GetAllUsers();
}

