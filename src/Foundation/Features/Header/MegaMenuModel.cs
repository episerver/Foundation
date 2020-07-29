using System.Collections.Generic;

namespace Foundation.Features.Header
{
    public class MegaMenuModel
    {
        public MegaMenuModel()
        {
            MenuItems = new List<MegaMenuItem>();
        }

        public IList<MegaMenuItem> MenuItems { get; set; }
    }

    public class MegaMenuItem
    {
        public MegaMenuItem()
        {
            Children = new List<MegaMenuItem>();
        }

        public string Url { get; set; }
        public string DisplayName { get; set; }
        public string ImageUrl { get; set; }
        public string Description { get; set; }
        public IList<MegaMenuItem> Children { get; set; }
    }
}