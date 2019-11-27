using System.ComponentModel;

namespace Foundation.Find.Cms.Config
{
    public class EnumSelectionDescriptionAttribute : DescriptionAttribute
    {
        public string Text { get; set; }
        public object Value { get; set; }
    }
}
