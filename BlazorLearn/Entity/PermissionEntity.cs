using System.ComponentModel;
using FreeSql;
using FreeSql.DataAnnotations;

namespace BlazorLearn.Entity;

[Description("权限表")]
public class PermissionEntity: BaseEntity<PermissionEntity, int>
{
    [Description("权限名")]
    public string? Name { get; set; }

    [Description("对应页面Url")]
    public string? Url { get; set; }
    
    [Description("角色")]
    [Navigate(ManyToMany = typeof(RolePermissionEntity))]
    public virtual ICollection<RoleEntity>? Roles { get; set; }
}