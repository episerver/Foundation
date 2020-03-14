using Foundation.Commerce.ViewModels.Header;
using Foundation.Demo.Models;
using System.Collections.Generic;

namespace Foundation.Demo.ViewModels
{
    public class DemoHeaderViewModel : CommerceHeaderViewModel
    {
        public bool LargeHeaderMenu { get; set; }
        public bool ShowCommerceControls { get; set; }
        public DemoHomePage DemoHomePage => HomePage as DemoHomePage;
        public List<DemoUserViewModel> DemoUsers { get; set; }
    }
}
