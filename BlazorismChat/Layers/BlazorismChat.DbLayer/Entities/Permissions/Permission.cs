using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BlazorismChat.DbLayer.Entities.Permissions;

public record Permission
{
    public Permission()
    {

    }

    public Permission(string permissionTitle, int? parentPermissionId = null)
    {
        PermissionTitle = permissionTitle;
        ParentPermissionId = parentPermissionId;
    }

    public Permission(int permissionId, string permissionTitle, int? parentPermissionId = null) : this(permissionTitle, parentPermissionId)
    {
        PermissionId = permissionId;
    }

    [Key] public int PermissionId { get; set; }

    [Display(Name = "عنوان دسترسی")]
    [Required(ErrorMessage = "{0} نمیتواند خالی باشد")]
    [MaxLength(200, ErrorMessage = "{0} نمیتواند بیش از {1} کاراکتر باشد")]
    [MinLength(3, ErrorMessage = "فیلد {0} باید حداقل {1} کاراکتر باشد.")]
    public string PermissionTitle { get; set; }

    public int? ParentPermissionId { get; set; }



    #region Relations

    [ForeignKey(nameof(ParentPermissionId))]
    public ICollection<Permission> SubPermissions { get; set; }
    public ICollection<RolePermission> RolePermissions { get; set; }

    #endregion
}