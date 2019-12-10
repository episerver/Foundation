using EPiServer.DataAbstraction;
using EPiServer.DataAnnotations;
using System.ComponentModel.DataAnnotations;

namespace Foundation.Cms.Pages
{
    public abstract class MailBasePage : FoundationPageData
    {
        [CultureSpecific]
        [Display(Name = "Subject", GroupName = SystemTabNames.Content, Order = 1)]
        public virtual string Subject { get; set; }
    }
}