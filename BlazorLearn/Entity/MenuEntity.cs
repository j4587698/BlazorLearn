using System.ComponentModel;
using FreeSql;
using FreeSql.DataAnnotations;

namespace BlazorLearn.Entity;

[Description("菜单表")]
public class MenuEntity: BaseEntity<MenuEntity, int>
{
    [Description("菜单名")]
    public string? Name { get; set; }

    [Description("菜单图标")]
    public string? Icon { get; set; }

    [Description("对应页面Url")]
    public string? Url { get; set; }

    [Description("父菜单ID")]
    public int ParentId { get; set; }

    [Navigate(nameof(ParentId))]
    public MenuEntity? Parent { get; set; }

    [Navigate(nameof(ParentId))]
    public List<MenuEntity>? Children { get; set; }
    
    [Description("角色")]
    [Navigate(ManyToMany = typeof(RoleMenuEntity))]
    public virtual ICollection<RoleEntity>? Roles { get; set; }
}