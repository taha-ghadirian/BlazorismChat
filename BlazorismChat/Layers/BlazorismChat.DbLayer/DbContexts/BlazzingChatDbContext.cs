using BlazorismChat.DbLayer.Entities.Permissions;
using BlazorismChat.DbLayer.Entities.Users;
using Microsoft.EntityFrameworkCore;

namespace BlazorismChat.DbLayer.DbContexts;

public class BlazzingChatDbContext : DbContext
{
    public BlazzingChatDbContext(DbContextOptions<BlazzingChatDbContext> options) : base(options)
    {

    }

    public DbSet<User> Users { get; set; }
    public DbSet<UserRole> UserRoles { get; set; }
    public DbSet<Role> Roles { get; set; }

    public DbSet<Permission> Permissions { get; set; }
    public DbSet<RolePermission> RolePermissions { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        #region User

        modelBuilder.Entity<User>(p =>
        {
            p.HasQueryFilter(u => !u.IsDeleted);
        });

        modelBuilder.Entity<Role>(p =>
        {
            p.HasQueryFilter(u => !u.IsDeleted);
        });

        modelBuilder.Entity<UserRole>(p =>
        {
            p.HasQueryFilter(u => !u.User.IsDeleted && !u.Role.IsDeleted);
        });

        #endregion

        #region Permissions

        modelBuilder.Entity<Permission>(p =>
        {
            p.HasData(new[]
            {
                new Permission(1,"پنل مدیریت"),
                //----------------------------------------------------------
                new Permission(2,"مدیریت کاربران", 1),
                new Permission(3,"افزودن کاربر", 2),
                new Permission(4,"ویرایش کاربر", 2),
                new Permission(5,"حذف کاربر", 4),
                new Permission(6,"تغییر رمز عبور کاربر", 4),
                //----------------------------------------------------------
                new Permission(7,"مدیریت نقش ها", 1),
                new Permission(8,"افزودن نقش جدید", 7),
                new Permission(9,"ویرایش نقش", 7),
                new Permission(10,"حذف نقش", 9),
                //----------------------------------------------------------
                new Permission(11,"مدیریت تیکت ها", 1),
                new Permission(12,"افزودن تیکت", 11),
                new Permission(13,"پاسخ تیکت", 11),
                new Permission(14,"تغییر وضعیت تیکت (همه وضعیت ها)", 13),
                new Permission(15,"بستن تیکت", 13),
                //----------------------------------------------------------

            });
        });

        modelBuilder.Entity<RolePermission>(p =>
        {
            p.HasQueryFilter(u => !u.Role.IsDeleted);
        });

        #endregion

        base.OnModelCreating(modelBuilder);
    }
}