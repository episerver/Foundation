using Microsoft.AspNet.Identity;

namespace Foundation.Commerce.Customer
{
    public class IdentityContactResult
    {
        public IdentityResult IdentityResult { get; set; }
        public FoundationContact FoundationContact { get; set; }
        public IdentityContactResult() => IdentityResult = new IdentityResult();
    }
}
