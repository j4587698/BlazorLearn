using System.Diagnostics.CodeAnalysis;
using BootstrapBlazor.Components;
using Furion.ClayObject;
using Microsoft.AspNetCore.Components;

namespace BlazorLearn.Pages.Account;

public partial class Login
{
    private string Title { get; set; } = "登录";

    [SupplyParameterFromQuery]
    [Parameter]
    public string? ReturnUrl { get; set; }

    private LoginVo LoginVo { get; set; } = new LoginVo();

    [Inject]
    [NotNull]
    private AjaxService? AjaxService { get; set; }
    
    [Inject]
    [NotNull]
    public MessageService? MessageService { get; set; }

    private Task OnSignUp()
    {
        throw new NotImplementedException();
    }

    private Task OnForgotPassword()
    {
        throw new NotImplementedException();
    }

    private async Task DoLogin()
    {
        if (string.IsNullOrEmpty(LoginVo.UserName))
        {
            await MessageService.Show(new MessageOption()
            {
                Color = Color.Danger,
                Content = "用户名不能为空"
            });
            return;
        }
        
        if (string.IsNullOrEmpty(LoginVo.Password))
        {
            await MessageService.Show(new MessageOption()
            {
                Color = Color.Danger,
                Content = "密码不能为空"
            });
            return;
        }
        
        var ajaxOption = new AjaxOption
        {
            Url = "/api/account/login",
            Data = LoginVo
        };
        var str = await AjaxService.GetMessage(ajaxOption);
        if (string.IsNullOrEmpty(str))
        {
            await MessageService.Show(new MessageOption()
            {
                Color = Color.Danger,
                Content = "登录失败"
            });
        }
        else
        {
            dynamic ret = Clay.Parse(str);
            if (ret.code != 20000)
            {
                await MessageService.Show(new MessageOption()
                {
                    Color = Color.Danger,
                    Content = ret.message
                });
            }
            else
            {
                await MessageService.Show(new MessageOption()
                {
                    Color = Color.Success,
                    Content = "登录成功"
                });
                ReturnUrl ??= "/";
                await AjaxService.Goto(ReturnUrl);
            }
        }
    }
}

public class LoginVo
{
    public string? UserName { get; set; }

    public string? Password { get; set; }

    public bool RememberMe { get; set; }
}