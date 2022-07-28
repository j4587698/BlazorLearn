using System.ComponentModel;
using FreeSql;
using FreeSql.DataAnnotations;

namespace BlazorLearn.Entity;

[Description("角色权限多多关系表")]
public class RolePermissionEntity : BaseEntity<RolePermissionEntity, Guid>
{
    [Description("角色Id")]
    public int RoleId { get; set; }

    [Description("角色")]
    [Navigate(nameof(RoleId))]
    public RoleEntity? Role { get; set; }

    [Description("权限Id")]
    public int PermissionId { get; set; }

    [Description("权限")]
    [Navigate(nameof(PermissionId))]
    public PermissionEntity? Permission { get; set; }
}