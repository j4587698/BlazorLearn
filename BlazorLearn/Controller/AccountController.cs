using System.Security.Claims;
using BlazorLearn.Entity;
using BlazorLearn.Pages.Account;
using BootstrapBlazor.Components;
using Furion.DataEncryption;
using Furion.DynamicApiController;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BlazorLearn.Controller;

public class AccountController: IDynamicApiController
{
    public async Task<object> PostLogin([FromBody]LoginVo loginVo)
    {
        if (string.IsNullOrEmpty(loginVo.UserName))
        {
            return new { code = 50000, message = "用户名不能为空" };
        }
        if (string.IsNullOrEmpty(loginVo.Password))
        {
            return new { code = 50000, message = "密码不能为空" };
        }

        var password = MD5Encryption.Encrypt(loginVo.Password);
        var user = await UserEntity.Where(x =>
            x.UserName == loginVo.UserName && x.Password == password).Include(x => x.Role).FirstAsync();
        if (user != null)
        {
            var identity = new ClaimsIdentity(CookieAuthenticationDefaults.AuthenticationScheme);
            identity.AddClaim(new Claim(ClaimTypes.Name, user.UserName!));
            identity.AddClaim(new Claim(ClaimTypes.Role, user.Role!.Id.ToString()));
            await Furion.App.HttpContext.SignInAsync(new ClaimsPrincipal(identity), new AuthenticationProperties(){IsPersistent = true, ExpiresUtc = loginVo.RememberMe? DateTimeOffset.Now.AddDays(5): DateTimeOffset.Now.AddMinutes(30)});

            return new { code = 20000, message = "登录成功" };
        }
        return new { code = 50000, message = "用户名或密码错误" };
    }

    [Authorize]
    public async Task<IActionResult> GetLogout()
    {
        await Furion.App.HttpContext.SignOutAsync();
        return new RedirectResult("/Login");
    }
}