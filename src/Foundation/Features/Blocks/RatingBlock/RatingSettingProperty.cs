using EPiServer.Core;
using EPiServer.PlugIn;
using Newtonsoft.Json;

namespace Foundation.Features.Blocks.RatingBlock
{
    /// <summary>
    /// This class maps the RatingSetting type to a property definition type so it can be
    /// used by the rating block.
    /// </summary>
    [PropertyDefinitionTypePlugIn]
    public class RatingSettingProperty : PropertyList<RatingSetting>
    {
        /// <summary>
        /// Overrides the base implementation of this method to
        /// parse a string to an instance of the RatingSetting object.
        /// </summary>
        /// <param name="value">the string representation to parse to an instance of RatingSetting</param>
        /// <returns>an instance of the RatingSetting object</returns>
        protected override RatingSetting ParseItem(string value) => JsonConvert.DeserializeObject<RatingSetting>(value);
    }
}