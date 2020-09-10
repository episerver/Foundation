using EPiServer.Core;
using Foundation.Features.Blog.BlogItemPage;
using Foundation.Features.LandingPages.LandingPage;
using Foundation.Features.Shared;
using Foundation.Features.StandardPage;
using PowerSlice;

namespace Foundation.Infrastructure.PowerSlices
{
    public class LandingPagesSlice : ContentSliceBase<LandingPage>
    {
        public override string Name => "Landing pages";

        public override int SortOrder => 10;
    }

    public class StandardPagesSlice : ContentSliceBase<StandardPage>
    {
        public override string Name => "Standard Pages";

        public override int SortOrder => 11;
    }

    public class BlogsSlice : ContentSliceBase<BlogItemPage>
    {
        public override string Name => "Blogs";

        public override int SortOrder => 12;
    }

    public class BlocksSlice : ContentSliceBase<FoundationBlockData>
    {
        public override string Name => "Blocks";

        public override int SortOrder => 50;
    }

    public class MediaSlice : ContentSliceBase<MediaData>
    {
        public override string Name => "Media";

        public override int SortOrder => 70;
    }

    public class ImagesSlice : ContentSliceBase<ImageData>
    {
        public override string Name => "Images";

        public override int SortOrder => 71;
    }
}