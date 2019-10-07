using Foundation.Cms.Blocks;
using System.Collections.Generic;

namespace Foundation.Cms.ViewModels.Blocks
{
    public class TagCloudBlockModel : BlockViewModel<TagCloudBlock>
    {
        public TagCloudBlockModel(TagCloudBlock block) : base(block) => Heading = block.Heading;

        public string Heading { get; set; }

        public IEnumerable<BlogItemPageModel.TagItem> Tags { get; set; }

    }
}
