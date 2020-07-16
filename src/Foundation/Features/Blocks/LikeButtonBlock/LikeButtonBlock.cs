using EPiServer.DataAnnotations;
using Foundation.Features.Shared;
using Foundation.Infrastructure;
namespace Foundation.Features.Blocks.LikeButtonBlock
{
    /// <summary>
    /// The LikeButtonBlock class defines the configuration used for rendering like button views.
    /// </summary>
    [ContentType(DisplayName = "Like Button Block",
                 GUID = "1dae01b7-72ad-4a9d-b543-82b0f5af7bbc",
                 Description = "A Like Button block implementation using the Episerver Social Ratings feature.", GroupName = GroupNames.Social)]
    [ImageUrl("~/assets/icons/cms/blocks/cms-icon-block-25.png")]
    public class LikeButtonBlock : FoundationBlockData
    {
    }
}