using EPiServer.Core;
using EPiServer.PlugIn;

namespace Foundation.Cms
{
    public class SelectionItem
    {
        public virtual string Text { get; set; }
        public virtual string Value { get; set; }
    }

    [PropertyDefinitionTypePlugIn]
    public class SelectionItemProperty : PropertyList<SelectionItem>
    {
    }
}
