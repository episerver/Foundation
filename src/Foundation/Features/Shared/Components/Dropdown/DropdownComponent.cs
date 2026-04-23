namespace Foundation.Features.Shared.Components.Dropdown
{
    public class DropdownViewComponent : ViewComponent
    {
        public IViewComponentResult Invoke(IEnumerable<KeyValuePair<string, string>> list,
              string selectedValue = "",
              string selectorClassItem = "",
              string name = "",
              bool isDisabled = false,
              bool isShow = true)
        {
            var model = new DropdownModel(list, selectedValue, selectorClassItem, name, isDisabled, isShow);
            return View("~/Features/Shared/Components/Dropdown/Default.cshtml", model);
        }
    }

    public class DropdownModel
    {
        public DropdownModel()
        {

        }

        public DropdownModel(IEnumerable<KeyValuePair<string, string>> list,
              string selectedValue = "",
              string selectorClassItem = "",
              string name = "",
              bool isDisabled = false,
              bool isShow = true)
        {
            List = list;
            SelectedValue = selectedValue;
            SelectorClassItem = selectorClassItem;
            Name = name;
            IsDisabled = isDisabled;
            IsShow = isShow;
        }

        public IEnumerable<KeyValuePair<string, string>> List { get; set; }
        public string SelectedValue { get; set; }
        public string SelectorClassItem { get; set; }
        public string Name { get; set; }
        public bool IsDisabled { get; set; }
        public bool IsShow { get; set; }
    }
}
