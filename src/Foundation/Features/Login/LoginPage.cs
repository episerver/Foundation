using EPiServer.Core;
using EPiServer.DataAbstraction;
using EPiServer.DataAnnotations;
using Foundation.Features.Shared;
using Foundation.Infrastructure;
using System.ComponentModel.DataAnnotations;

namespace Foundation.Features.Login;

[ContentType(DisplayName = "Login Page",
    GUID = "0C21EBEE-521C-4391-A429-BA6B178DDC6A",
    Description = "Default login page that has top content area, main body, and main content area",
    GroupName = GroupNames.Content)]
[ImageUrl("/icons/gfx/login.jpg")]
public class LoginPage : FoundationPageData
{
    [Display(Name = "Top content area", GroupName = SystemTabNames.Content, Order = 90)]
    public virtual ContentArea TopContentArea { get; set; }
}