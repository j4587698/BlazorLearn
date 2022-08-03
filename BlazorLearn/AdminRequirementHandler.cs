using System.Security.Claims;
using BlazorLearn.Entity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components;
using RouteData = Microsoft.AspNetCore.Components.RouteData;

namespace BlazorLearn;

public class AdminRequirementHandler : AuthorizationHandler<AdminRequirement>
{
    protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, AdminRequirement requirement)
    {
        if (context.User.Identity?.IsAuthenticated != true)
        {
            return Task.CompletedTask;
        }

        if (!int.TryParse(context.User.FindFirst(ClaimTypes.Role)?.Value, out var roleId))
        {
            return Task.CompletedTask;
        }
        if (context.Resource is RouteData routeData)
        {
            var routeAttr = routeData.PageType.CustomAttributes.FirstOrDefault(x =>
                x.AttributeType == typeof(RouteAttribute));
            if (routeAttr == null)
            {
                context.Succeed(requirement);
            }
            else
            {
                var url = routeAttr.ConstructorArguments[0].Value as string;
                var permission = PermissionEntity
                    .Where(x => x.Roles!.Any(y => y.Id == roleId) && x.Url == url).First();
                if (permission != null)
                {
                    context.Succeed(requirement);
                }
            }
        }
        
        return Task.CompletedTask;
    }
}