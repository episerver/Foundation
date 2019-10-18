using EPiServer.Core;
using EPiServer.DataAbstraction;
using EPiServer.DataAnnotations;
using EPiServer.Validation;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Foundation.Cms.Pages
{
    [ContentType(DisplayName = "Three Column Landing Page",
       GUID = "947EDF31-8C8C-4595-8591-A17DEF75685E",
       Description = "Three column landing page with properties to determin column size",
       GroupName = SystemTabNames.Content)]
    [ImageUrl("~/assets/icons/gfx/page-type-thumbnail-landingpage-threecol.png")]
    public class ThreeColumnLandingPage : LandingPage
    {
        [CultureSpecific]
        [Display(Name = "Left content area", GroupName = SystemTabNames.Content, Order = 190)]
        public virtual ContentArea LeftHandContentArea { get; set; }

        [CultureSpecific]
        [Display(Name = "Right content area", GroupName = SystemTabNames.Content, Order = 210)]
        public virtual ContentArea RightHandContentArea { get; set; }

        [CultureSpecific]
        [Display(Name = "Left column", GroupName = SystemTabNames.Content, Order = 220)]
        public virtual int MainContentAreaColumn { get; set; }

        [CultureSpecific]
        [Display(Name = "Center column", GroupName = SystemTabNames.Content, Order = 221)]
        public virtual int RightHandContentAreaColumn { get; set; }

        [CultureSpecific]
        [Display(Name = "Right column", GroupName = SystemTabNames.Content, Order = 222)]
        public virtual int LeftHandContentAreaColumn { get; set; }

        public override void SetDefaultValues(ContentType contentType)
        {
            base.SetDefaultValues(contentType);

            MainContentAreaColumn = RightHandContentAreaColumn = this.LeftHandContentAreaColumn = 4;
        }
    }

    public class ThreeColumnLandingPageValidation : IValidate<ThreeColumnLandingPage>
    {
        public IEnumerable<ValidationError> Validate(ThreeColumnLandingPage instance)
        {
            var validations = new List<ValidationError>();
            if (instance.MainContentAreaColumn + instance.RightHandContentAreaColumn + instance.LeftHandContentAreaColumn != 12)
            {
                var error = new ValidationError();
                error.PropertyName = nameof(instance.MainContentAreaColumn) + ", " + nameof(instance.RightHandContentAreaColumn) + ", " + nameof(instance.LeftHandContentAreaColumn);
                error.ErrorMessage = "Sum of columns must be 12. Properties " + error.PropertyName;
                error.Severity = ValidationErrorSeverity.Error;
                error.RelatedProperties = new List<string> { nameof(instance.MainContentAreaColumn), nameof(instance.RightHandContentAreaColumn), nameof(instance.LeftHandContentAreaColumn) };
                validations.Add(error);
            }

            if (instance.MainContentAreaColumn < 1)
            {
                var error = new ValidationError();
                error.PropertyName = nameof(instance.MainContentAreaColumn);
                error.ErrorMessage = "Value must be greater than 0. Properties " + error.PropertyName;
                error.Severity = ValidationErrorSeverity.Error;
                error.RelatedProperties = new List<string> { nameof(instance.MainContentAreaColumn) };
                validations.Add(error);
            }

            if (instance.RightHandContentAreaColumn < 1)
            {
                var error = new ValidationError();
                error.PropertyName = nameof(instance.RightHandContentAreaColumn);
                error.ErrorMessage = "Value must be greater than 0. Properties " + error.PropertyName;
                error.Severity = ValidationErrorSeverity.Error;
                error.RelatedProperties = new List<string> { nameof(instance.RightHandContentAreaColumn) };
                validations.Add(error);
            }

            if (instance.LeftHandContentAreaColumn < 1)
            {
                var error = new ValidationError();
                error.PropertyName = nameof(instance.LeftHandContentAreaColumn);
                error.ErrorMessage = "Value must be greater than 0. Properties " + error.PropertyName;
                error.Severity = ValidationErrorSeverity.Error;
                error.RelatedProperties = new List<string> { nameof(instance.LeftHandContentAreaColumn) };
                validations.Add(error);
            }

            return validations;
        }
    }
}
