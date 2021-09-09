using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using BlazorismChat.DbLayer.Entities.Permissions;

namespace BlazorismChat.DbLayer.Entities.Users;

public record Role
{
    public Role()
    {

    }

    public Role(string roleName)
    {
        RoleName = roleName;
    }

    [Key] public int RoleId { get; set; }

    [Display(Name = "مقام")]
    [Required(ErrorMessage = "{0} نمیتواند خالی باشد")]
    [MaxLength(50, ErrorMessage = "{0} نمیتواند بیش از {1} کاراکتر باشد")]
    [MinLength(3, ErrorMessage = "فیلد {0} باید حداقل {1} کاراکتر باشد.")]
    public string RoleName { get; set; }

    public bool IsDeleted { get; set; }

    #region Relations

    public ICollection<UserRole> UserRoles { get; set; }
    public ICollection<RolePermission> RolePermissions { get; set; }

    #endregion
}