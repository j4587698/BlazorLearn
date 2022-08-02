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
            

            PermissionEntity homePermission = new PermissionEntity()
            {
                Name = "首页",
                Url = ""
            };
            homePermission.Save();
            
            PermissionEntity userPermission = new PermissionEntity()
            {
                Name = "用户管理",
                Url = "User"
            };
            userPermission.Save();
            
            RoleEntity role = new RoleEntity()
            {
                Name = "管理员",
                Users = new List<UserEntity>() { user },
                Permissions = new List<PermissionEntity>(){homePermission, userPermission}
            };
            role.Save().SaveMany(nameof(RoleEntity.Users));
            role.SaveMany(nameof(RoleEntity.Permissions));
        }
        
        
        return services;
    }
}