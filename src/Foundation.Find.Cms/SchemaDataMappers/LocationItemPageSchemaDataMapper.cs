using Foundation.Cms.SchemaMarkup;
using Foundation.Find.Cms.Models.Pages;
using Schema.NET;

namespace Foundation.Find.Cms.SchemaDataMappers
{
    /// <summary>
    /// Map LocationItemPage to Schema.org location objects
    /// </summary>
    public class LocationItemPageSchemaDataMapper : ISchemaDataMapper<LocationItemPage>
    {
        public Thing Map(LocationItemPage content)
        {
            return new AdministrativeArea
            {
                Name = content.Name,
                ContainedInPlace = new Country
                {
                    Name = content.Country
                },
                Geo = new GeoCoordinates
                {
                    Latitude = content.Latitude,
                    Longitude = content.Longitude,
                    AddressCountry = content.Country
                }
            };
        }
    }
}
