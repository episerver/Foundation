using EPiServer.Core;
using EPiServer.DataAbstraction;
using EPiServer.DataAnnotations;
using EPiServer.Validation;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Foundation.Features.LandingPages.ThreeColumnLandingPage
{
    [ContentType(DisplayName = "Three Column Landing Page",
       GUID = "947EDF31-8C8C-4595-8591-A17DEF75685E",
       Description = "Three column landing page with properties to determin column size",
       GroupName = SystemTabNames.Content)]
    [ImageUrl("~/assets/icons/gfx/page-type-thumbnail-landingpage-threecol.png")]
    public class ThreeColumnLandingPage : LandingPage.LandingPage
    {
        [CultureSpecific]
        [Display(Name = "Left content area", GroupName = SystemTabNames.Content, Order = 190)]
        public virtual ContentArea LeftContentArea { get; set; }

        [CultureSpecific]
        [Display(Name = "Right content area", GroupName = SystemTabNames.Content, Order = 210)]
        public virtual ContentArea RightContentArea { get; set; }

        [CultureSpecific]
        [Display(Name = "Left column", GroupName = SystemTabNames.Content, Order = 220)]
        public virtual int LeftColumn { get; set; }

        [CultureSpecific]
        [Display(Name = "Center column", GroupName = SystemTabNames.Content, Order = 221)]
        public virtual int CenterColumn { get; set; }

        [CultureSpecific]
        [Display(Name = "Right column", GroupName = SystemTabNames.Content, Order = 222)]
        public virtual int RightColumn { get; set; }

        public override void SetDefaultValues(ContentType contentType)
        {
            base.SetDefaultValues(contentType);

            LeftColumn = CenterColumn = RightColumn = 4;
        }
    }

    public class ThreeColumnLandingPageValidation : IValidate<ThreeColumnLandingPage>
    {
        public IEnumerable<ValidationError> Validate(ThreeColumnLandingPage instance)
        {
            var validations = new List<ValidationError>();
            if (instance.LeftColumn + instance.CenterColumn + instance.RightColumn != 12)
            {
                var error = new ValidationError
                {
                    PropertyName = nameof(instance.LeftColumn) + ", " + nameof(instance.CenterColumn) + ", " + nameof(instance.RightColumn)
                };
                error.ErrorMessage = "Sum of columns must be 12. Properties " + error.PropertyName;
                error.Severity = ValidationErrorSeverity.Error;
                error.RelatedProperties = new List<string> { nameof(instance.LeftColumn), nameof(instance.CenterColumn), nameof(instance.RightColumn) };
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

            if (instance.CenterColumn < 1)
            {
                var error = new ValidationError
                {
                    PropertyName = nameof(instance.CenterColumn)
                };
                error.ErrorMessage = "Value must be greater than 0. Properties " + error.PropertyName;
                error.Severity = ValidationErrorSeverity.Error;
                error.RelatedProperties = new List<string> { nameof(instance.CenterColumn) };
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