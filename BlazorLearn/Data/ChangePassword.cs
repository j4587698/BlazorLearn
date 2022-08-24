using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using BootstrapBlazor.Components;

namespace BlazorLearn.Data;

public class ChangePassword
{
    [Display(Name = "原密码")]
    [Required(ErrorMessage = "原密码不能为空")]
    [AutoGenerateColumn(ComponentType = typeof(BootstrapPassword))]
    public string? OldPassword { get; set; }

    [Display(Name = "新密码")]
    [Required(ErrorMessage = "新密码不能为空")]
    [AutoGenerateColumn(ComponentType = typeof(BootstrapPassword))]
    public string? NewPassword { get; set; }
    
    [Display(Name = "重复新密码")]
    [Compare(nameof(NewPassword), ErrorMessage = "两次密码不一致")]
    [AutoGenerateColumn(ComponentType = typeof(BootstrapPassword))]
    public string? RePassword { get; set; }
}