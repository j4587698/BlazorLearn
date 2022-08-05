using System.ComponentModel;
using FreeSql;
using FreeSql.DataAnnotations;

namespace BlazorLearn.Entity;

[Description("角色表")]
public class RoleEntity : BaseEntity<RoleEntity, int>
{
    [Description("角色名称")]
    public string? Name { get; set; }

    [Description("用户")]
    [Navigate(nameof(UserEntity.RoleId))]
    public virtual ICollection<UserEntity>? Users { get; set; }

    [Description("权限")]
    [Navigate(ManyToMany = typeof(RoleMenuEntity))]
    public virtual ICollection<MenuEntity>? Permissions { get; set; }
}