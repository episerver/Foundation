using Foundation.Social.Models.Groups;
using System.Collections.Generic;

namespace Foundation.Social.Repositories.Groups
{
    /// <summary>
    ///     The ICommunityRepository interface describes a component capable
    ///     of persisting, and retrieving community data
    /// </summary>
    public interface ICommunityRepository
    {
        /// <summary>
        ///     Adds a community to the underlying community repository.
        /// </summary>
        /// <param name="community">The community to add.</param>
        /// <returns>The added community.</returns>
        Community Add(Community community);

        /// <summary>
        ///     Updates a community to the underlying community repository.
        /// </summary>
        /// <param name="community">The updated community.</param>
        /// <returns>The updated community.</returns>
        Community Update(Community community);

        /// <summary>
        ///     Retrieves a community based on the name of the community provided.
        /// </summary>
        /// <param name="communityName">The name of the community that is to be retrieved from the underlying data store.</param>
        /// <returns>The desired community.</returns>
        Community Get(string communityName);

        /// <summary>
        ///     Retrieves a community based on a list of community ids that are provided.
        /// </summary>
        /// <param name="communityIds">The communitys ids that are to be retrieved from the underlying data store.</param>
        /// <returns>The requested communitys.</returns>
        List<Community> Get(List<string> communityIds);
    }
}