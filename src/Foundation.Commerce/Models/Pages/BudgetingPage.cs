using EPiServer.DataAnnotations;
using Foundation.Cms.EditorDescriptors;
using Foundation.Cms.Pages;

namespace Foundation.Commerce.Models.Pages
{
    [ContentType(DisplayName = "Budgeting Page",
        GUID = "0ad21ec9-3753-4e2f-9ee8-61e8cba45fe3",
        Description = "Manage budgets for organization.",
        GroupName = CommerceGroupNames.Commerce,
        AvailableInEditMode = false)]
    [ImageUrl("~/assets/icons/cms/pages/elected.png")]
    public class BudgetingPage : FoundationPageData, IDisableOPE
    {
    }
}