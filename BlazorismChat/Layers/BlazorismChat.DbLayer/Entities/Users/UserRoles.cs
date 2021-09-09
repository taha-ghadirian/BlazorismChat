using System.ComponentModel.DataAnnotations;

namespace BlazorismChat.DbLayer.Entities.Users;

public record UserRole
{
    public UserRole()
    {

    }

    public UserRole(int userId, int roleId)
    {
        UserId = userId;
        RoleId = roleId;
    }

    [Key] public int UserRoleId { get; set; }

    public int UserId { get; set; }

    public int RoleId { get; set; }

    #region Relations

    public User User { get; set; }
    public Role Role { get; set; }

    #endregion
}