using BlazorismChat.ClientLibraries.Convertors;
using BlazorismChat.Core.Security;
using BlazorismChat.Core.ServerServices;
using BlazorismChat.Core.ServerServices.Interfaces;
using BlazorismChat.DbLayer.DbContexts;
using BlazorismChat.DbLayer.Entities.Users;
using Bunit;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace BlazorismChat.CoreTests.Helpers;

public static class TestContextExtensions
{
    public static TestContext AddBlazorismChatSupport(this TestContext context)
    {
        context.Services.AddDbContext<BlazorismChatDbContext>(p =>
        {
#if DEBUG
            p.UseInMemoryDatabase(NameGenerator.GenerateUniqueCode());
#else
            var connectionString = Environment.GetEnvironmentVariable("CONNECTION_STRING__TEST") ?? "";
            p.UseSqlServer(connectionString);
#endif
        });

        context.Services.AddTransient<IUserService, UserService>();

        return context;
    }

    public static void FirstInitDb(this BlazorismChatDbContext db)
    {
        db.Database.EnsureDeleted();
        db.Database.EnsureCreated();

        db.Users.AddRange(new[]
        {
            new User
            {
                ActiveCode = NameGenerator.GenerateUniqueCode(),
                IdentityCode = NameGenerator.GenerateUniqueCode(),
                Email = "test1@gmail.com",
                FirstName = "test",
                LastName = "1",
                IsEmailConfirmed= true,
                IsPhoneNumberConfirmed = true,
                Password = PasswordHelper.EncodePasswordMd5("test1"),
                UserName = "test1",
                PhoneNumber = "09101230000",
            },
            new User
            {
                ActiveCode = NameGenerator.GenerateUniqueCode(),
                IdentityCode = NameGenerator.GenerateUniqueCode(),
                Email = "test2@gmail.com",
                FirstName = "test",
                LastName = "2",
                IsEmailConfirmed= true,
                IsPhoneNumberConfirmed = true,
                Password = PasswordHelper.EncodePasswordMd5("test2"),
                UserName = "test2",
                PhoneNumber = "09101230002",
            },
        });

        db.SaveChanges();
    }
}