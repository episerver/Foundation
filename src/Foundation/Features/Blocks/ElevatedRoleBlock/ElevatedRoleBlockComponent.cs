using Foundation.Features.CatalogContent;
using Foundation.Infrastructure.Commerce.Customer;
using Mediachase.Commerce.Customers;

namespace Foundation.Features.Blocks.ElevatedRoleBlock
{
    public class ElevatedRoleBlockComponent : AsyncBlockComponent<ElevatedRoleBlock>
    {
        protected override async Task<IViewComponentResult> InvokeComponentAsync(ElevatedRoleBlock currentBlock)
        {
            var viewModel = new ElevatedRoleBlockViewModel(currentBlock);
            var currentContact = CustomerContext.Current.CurrentContact;
            if (currentContact != null)
            {
                var contact = new FoundationContact(currentContact);
                if (contact.ElevatedRole == ElevatedRoles.Reader.ToString())
                {
                    viewModel.IsAccess = true;
                }
            }
            return await Task.FromResult(View("~/Features/Blocks/ElevatedRoleBlock/ElevatedRoleBlock.cshtml", viewModel));
        }
    }
}
