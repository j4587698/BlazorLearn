using System.Security.Claims;
using BlazorLearn.Entity;
using Furion.Authorization;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components;
using RouteData = Microsoft.AspNetCore.Components.RouteData;

namespace BlazorLearn;

public class AdminHandler : AppAuthorizeHandler
{
    public override Task<bool> PipelineAsync(AuthorizationHandlerContext context, DefaultHttpContext httpContext)
    {
        if (!int.TryParse(context.User.FindFirst(ClaimTypes.Role)?.Value, out var roleId))
        {
            return Task.FromResult(false);
        }
        if (context.Resource is RouteData routeData)
        {
            var routeAttr = routeData.PageType.CustomAttributes.FirstOrDefault(x =>
                x.AttributeType == typeof(RouteAttribute));
            if (routeAttr == null)
            {
                return Task.FromResult(true);
            }
            else
            {
                var url = routeAttr.ConstructorArguments[0].Value as string;
                var permission = PermissionEntity
                    .Where(x => x.Roles!.Any(y => y.Id == roleId) && x.Url == url).First();
                if (permission != null)
                {
                    return Task.FromResult(true);
                }
            }
        }
        
        return Task.FromResult(false);
    }
}