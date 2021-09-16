using BlazorismChat.ClientLibraries.Convertors;
using BlazorismChat.DbLayer.Entities.Permissions;
using BlazorismChat.DbLayer.Entities.Users;
using Microsoft.EntityFrameworkCore;

namespace BlazorismChat.DbLayer.DbContexts;

public class TestDb : BlazorismChatDbContext
{
    public TestDb(DbContextOptions options) : base(options)
    {

    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if (!optionsBuilder.IsConfigured)
        {
            //#if DEBUG
            //            optionsBuilder.UseInMemoryDatabase(NameGenerator.GenerateUniqueCode());
            //#else
            //var connectionString = Environment.GetEnvironmentVariable("CONNECTION_STRING__TEST") ?? "";
            optionsBuilder.UseSqlServer("data source=.; initial catalogBlazorismChatTEST; user id=sa; password=12345; multipleActiveResultSets=true;");
            //#endif
        }

        base.OnConfiguring(optionsBuilder);
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        #region Tests

        modelBuilder.Entity<User>(p =>
            {
                p.Property(nameof(User.FirstName)).IsRequired(false);
                p.Property(nameof(User.LastName)).IsRequired(false);
                p.Property(nameof(User.PhoneNumber)).IsRequired(false);
            }
        );

        #endregion

        base.OnModelCreating(modelBuilder);
    }
}