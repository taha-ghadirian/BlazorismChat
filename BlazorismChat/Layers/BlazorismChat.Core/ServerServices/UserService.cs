using BlazorismChat.Core.ServerServices.Interfaces;
using BlazorismChat.DbLayer.Entities.Users;
using BlazorismChat.DbLayer.DbContexts;
using Microsoft.EntityFrameworkCore;
using BlazorismChat.ClientLibraries.Convertors;
using BlazorismChat.Core.Security;

namespace BlazorismChat.Core.ServerServices;

public class UserService : IUserService
{
    private readonly BlazorismChatDbContext _dbContext;

    public UserService(BlazorismChatDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<User?> CreateUser(User user)
    {
        try
        {
            if(await IsUsernameExists(user.UserName) || await IsEmailExists(user.Email))
                return null;

            var result = await _dbContext.Users.AddAsync(user);
            await _dbContext.SaveChangesAsync();
            return result.Entity;
        }
        catch
        {
            return null;
        }
    }

    public async Task<bool> DeleteUser(User user, bool fullyDelete = false)
    {
        try
        {
            if (user == null)
                return false;

            if (fullyDelete)
            {
                _dbContext.Users.Remove(user);
                await _dbContext.SaveChangesAsync();
            }
            else
            {
                user.IsDeleted = true;
                var resultUser = await UpdateUser(user, true);
                return resultUser != null;
            }

            return true;
        }
        catch
        {
            return false;
        }
    }

    public async Task<bool> DeleteUser(int userId, bool fullyDelete = false)
    {
        var user = await GetUserById(userId);
        if (user == null)
            return false;

        return await DeleteUser(user, fullyDelete);
    }

    public async Task<List<User>> GetAllUsers()
    {
        return await _dbContext.Users.ToListAsync();
    }

    public async Task<User?> GetUserByEmail(string email)
    {
        return await _dbContext.Users.FirstOrDefaultAsync(x => x.FixedEmail == TextFixer.FixEmail(email));
    }

    public async Task<User?> GetUserById(int userId)
    {
        var user = await _dbContext.Users.FindAsync(userId);

        if (user == null)
            return null;

        return user.IsDeleted ? null : user;
    }

    public async Task<User?> GetUserByUsername(string username)
    {
        return await _dbContext.Users.FirstOrDefaultAsync(x => x.FixedUserName == TextFixer.FixUserName(username));
    }

    public async Task<bool> IsEmailExists(string email)
    {
        return await _dbContext.Users.AnyAsync(x => x.FixedEmail == TextFixer.FixEmail(email));
    }

    public async Task<bool> IsUserExists(int userId)
    {
        return await _dbContext.Users.AnyAsync(x => x.UserId == userId);
    }

    public async Task<bool> IsUsernameExists(string username)
    {
        return await _dbContext.Users.AnyAsync(x => x.FixedUserName == TextFixer.FixUserName(username));
    }

    public async Task<User?> LoginUser(string username, string password)
    {
        var encPass = PasswordHelper.EncodePasswordMd5(password);
        return await _dbContext.Users.FirstOrDefaultAsync(x => x.FixedUserName == TextFixer.FixUserName(username) && x.Password == encPass);
    }

    public async Task<User?> RegisterUser(User user)
    {
        try
        {
            if (await IsUsernameExists(user.UserName) || await IsEmailExists(user.Email))
                return null;

            user.Password = PasswordHelper.EncodePasswordMd5(user.Password);

#if DEBUG
            user.IsEmailConfirmed = true;
            user.IsPhoneNumberConfirmed = true;
#endif

            return await CreateUser(user);
        }
        catch
        {
            return null;
        }
    }

    public async Task<User?> UpdateUser(User user, bool updateSecurityKey = false)
    {
        try
        {
            if (updateSecurityKey)
                user.ActiveCode = NameGenerator.GenerateUniqueCode();

            user.IdentityCode = NameGenerator.GenerateUniqueCode();

            _dbContext.Entry(user).State = EntityState.Modified;
            await _dbContext.SaveChangesAsync();
            return user;
        }
        catch
        {
            return null;
        }
    }

    public async Task<bool> UpdateUserPassword(User user, string password)
    {
        try
        {
            if (user == null || user == new User())
                return false;

            if (string.IsNullOrWhiteSpace(password))
                return false;

            user.Password = PasswordHelper.EncodePasswordMd5(password);
            var result = await UpdateUser(user, true);
            return result != null;
        }
        catch
        {
            return false;
        }
    }

    public async Task<bool> UpdateUserPassword(string username, string password)
    {
        var user = await GetUserByUsername(username);
        if (user == null)
            return false;

        return await UpdateUserPassword(user, password);
    }
}