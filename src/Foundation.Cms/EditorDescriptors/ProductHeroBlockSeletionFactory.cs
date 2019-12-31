using EPiServer.Shell.ObjectEditing;
using System.Collections.Generic;
using System.Linq;

namespace Foundation.Cms.EditorDescriptors
{
    public class ProductHeroBlockLayoutSeletionFactory : ISelectionFactory
    {
        public IEnumerable<ISelectItem> GetSelections(ExtendedMetadata metadata)
        {
            var dic = new Dictionary<string, string>
            {
                { "Callout on the left - image on the right", "CalloutLeft" },
                { "Callout on the right - image on the left", "CalloutRight" },
            };

            return dic.Select(x => new SelectItem { Text = x.Key, Value = x.Value });
        }
    }

    public class ProductHeroBlockImagePositionSeletionFactory : ISelectionFactory
    {
        public IEnumerable<ISelectItem> GetSelections(ExtendedMetadata metadata)
        {
            var dic = new Dictionary<string, string>
            {
                { "Left", "ImageLeft" },
                { "Center", "ImageCenter" },
                { "Right", "ImageRight" },
                { "Use Paddings", "ImagePaddings" },
            };

            return dic.Select(x => new SelectItem { Text = x.Key, Value = x.Value });
        }
    }
}
