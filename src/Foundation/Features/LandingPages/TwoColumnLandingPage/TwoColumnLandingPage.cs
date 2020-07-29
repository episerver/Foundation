using EPiServer.Core;
using EPiServer.DataAbstraction;
using EPiServer.DataAnnotations;
using EPiServer.Validation;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Foundation.Features.LandingPages.TwoColumnLandingPage
{
    [ContentType(DisplayName = "Two Column Landing Page",
       GUID = "F94571B0-65C4-4E49-8A88-5930D045E19D",
       Description = "Two column landing page with properties to determine column size",
       GroupName = SystemTabNames.Content)]
    [ImageUrl("~/assets/icons/gfx/page-type-thumbnail-landingpage-twocol.png")]
    public class TwoColumnLandingPage : LandingPage.LandingPage
    {
        [CultureSpecific]
        [Display(Name = "Right content area", GroupName = SystemTabNames.Content, Order = 210)]
        public virtual ContentArea RightContentArea { get; set; }

        [CultureSpecific]
        [Display(Name = "Left column", GroupName = SystemTabNames.Content, Order = 220)]
        public virtual int LeftColumn { get; set; }

        [CultureSpecific]
        [Display(Name = "Right column", GroupName = SystemTabNames.Content, Order = 221)]
        public virtual int RightColumn { get; set; }

        public override void SetDefaultValues(ContentType contentType)
        {
            base.SetDefaultValues(contentType);

            LeftColumn = RightColumn = 6;
        }
    }

    public class TwoColumnLandingPageValidation : IValidate<TwoColumnLandingPage>
    {
        public IEnumerable<ValidationError> Validate(TwoColumnLandingPage instance)
        {
            var validations = new List<ValidationError>();
            if (instance.LeftColumn + instance.RightColumn != 12)
            {
                var error = new ValidationError
                {
                    PropertyName = nameof(instance.LeftColumn) + ", " + nameof(instance.RightColumn)
                };
                error.ErrorMessage = "Sum of columns must be 12. Properties " + error.PropertyName;
                error.Severity = ValidationErrorSeverity.Error;
                error.RelatedProperties = new List<string> { nameof(instance.LeftColumn), nameof(instance.RightColumn) };
                validations.Add(error);
            }

            if (instance.LeftColumn < 1)
            {
                var error = new ValidationError
                {
                    PropertyName = nameof(instance.LeftColumn)
                };
                error.ErrorMessage = "Value must be greater than 0. Properties " + error.PropertyName;
                error.Severity = ValidationErrorSeverity.Error;
                error.RelatedProperties = new List<string> { nameof(instance.LeftColumn) };
                validations.Add(error);
            }

            if (instance.RightColumn < 1)
            {
                var error = new ValidationError
                {
                    PropertyName = nameof(instance.RightColumn)
                };
                error.ErrorMessage = "Value must be greater than 0. Properties " + error.PropertyName;
                error.Severity = ValidationErrorSeverity.Error;
                error.RelatedProperties = new List<string> { nameof(instance.RightColumn) };
                validations.Add(error);
            }

            return validations;
        }
    }
}
