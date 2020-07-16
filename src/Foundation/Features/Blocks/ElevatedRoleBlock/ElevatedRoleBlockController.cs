using EPiServer.Framework.DataAnnotations;
using EPiServer.Web.Mvc;
using Foundation.Commerce.Customer;
using Foundation.Features.CatalogContent;
using Mediachase.Commerce.Customers;
using System.Web.Mvc;

namespace Foundation.Features.Blocks.ElevatedRoleBlock
{
    [TemplateDescriptor(Default = true)]
    public class ElevatedRoleBlockController : BlockController<ElevatedRoleBlock>
    {
        public override ActionResult Index(ElevatedRoleBlock currentBlock)
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
            return PartialView("~/Features/Blocks/ElevatedRoleBlock/ElevatedRoleBlock.cshtml", viewModel);
        }
    }
}
