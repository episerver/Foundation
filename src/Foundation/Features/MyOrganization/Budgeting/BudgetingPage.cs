using Foundation.Features.Shared.EditorDescriptors;

namespace Foundation.Features.MyOrganization.Budgeting
{
    [ContentType(DisplayName = "Budgeting Page",
        GUID = "0ad21ec9-3753-4e2f-9ee8-61e8cba45fe3",
        Description = "Manage budgets for organization.",
        GroupName = GroupNames.Commerce,
        AvailableInEditMode = false)]
    [ImageUrl("/icons/cms/pages/elected.png")]
    public class BudgetingPage : FoundationPageData, IDisableOPE
    {
    }
}