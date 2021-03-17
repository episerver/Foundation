using Foundation.Social.Models.Groups;
using System.Collections.Generic;

namespace Foundation.Social.Repositories.Groups
{
    /// <summary>
    ///     The ICommunityMemberRepository interface describes a component capable
    ///     of persisting, and retrieving community member data
    /// </summary>
    public interface ICommunityMemberRepository
    {
        /// <summary>
        ///     Adds a member to the underlying member repository.
        /// </summary>
        /// <param name="member">The member to add.</param>
        /// <returns>The added member.</returns>
        CommunityMember Add(CommunityMember member);

        /// <summary>
        ///     Retrieves a list of members to the underlying member repository.
        /// </summary>
        /// <param name="memberFilter">The filter by which to retrieve members by.</param>
        /// <returns>The list of members that are part of the specified group.</returns>
        IEnumerable<CommunityMember> Get(CommunityMemberFilter memberFilter);
    }
}