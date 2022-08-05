using BlazorLearn.Entity;
using FreeSql;
using Furion.DataEncryption;

namespace BlazorLearn.Extensions;

public static class DbExtension
{
    public static IServiceCollection AddDb(this IServiceCollection services)
    {
        var conn = Furion.App.Configuration["Db:ConnString"];
        var freeSql = new FreeSqlBuilder()
            .UseAutoSyncStructure(Furion.App.WebHostEnvironment.IsDevelopment())
            .UseConnectionString(DataType.Sqlite, conn)
            .Build();
        
        freeSql.Aop.ConfigEntity += (s, e) =>
        {
            e.ModifyResult.Name = e.EntityType.Name.Replace("Entity", "");
        };
        
        BaseEntity.Initialization(freeSql, null);

        if (!UserEntity.Where(x => x.UserName == "Admin").Any())
        {
            UserEntity user = new UserEntity()
            {
                UserName = "Admin",
                Password = MD5Encryption.Encrypt("Admin"),
                Name = "张三"
            };
            user.Save();
            

            MenuEntity homeMenu = new MenuEntity()
            {
                Name = "首页",
                Url = "/",
                Icon = "fa fa-home"
            };
            homeMenu.Save();

            var menuMenu = new MenuEntity()
            {
                Name = "菜单管理",
                Url = "/menu",
                Icon = "fa fa-bars",
                Sort = 10
            };
            menuMenu.Save();

            var roleMenu = new MenuEntity()
            {
                Name = "角色管理",
                Url = "/role",
                Icon = "fa fa-sitemap",
                Sort = 20
            };
            roleMenu.Save();
            
            MenuEntity userMenu = new MenuEntity()
            {
                Name = "用户管理",
                Url = "/user",
                Icon = "fa fa-user",
                Sort = 30
            };
            userMenu.Save();

            var parentTest = new MenuEntity()
            {
                Name = "父菜单",
                Url = "/parent"
            };
            parentTest.Save();

            var child1 = new MenuEntity()
            {
                Name = "子菜单1",
                Url = "/child1",
                ParentId = parentTest.Id
            };
            child1.Save();

            var child2 = new MenuEntity()
            {
                Name = "子菜单2",
                Url = "/child2",
                ParentId = parentTest.Id
            };
            child2.Save();
            
            RoleEntity role = new RoleEntity()
            {
                Name = "管理员",
                Users = new List<UserEntity>() { user },
                Permissions = new List<MenuEntity>(){homeMenu, menuMenu, roleMenu, userMenu, parentTest, child1, child2}
            };
            role.Save().SaveMany(nameof(RoleEntity.Users));
            role.SaveMany(nameof(RoleEntity.Permissions));
        }
        
        
        return services;
    }
}