using EPiServer.Core;
using EPiServer.DataAbstraction;
using EPiServer.DataAnnotations;
using EPiServer.Validation;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Foundation.Cms.Pages
{
    [ContentType(
       GUID = "F94571B0-65C4-4E49-8A88-5930D045E19D",
       DisplayName = "Two Column Landing Page",
       Description = "Two column landing page with properties to determine column size",
       GroupName = CmsTabs.Content)]
    [ImageUrl("~/assets/icons/gfx/page-type-thumbnail-landingpage-twocol.png")]
    public class TwoColumnLandingPage : LandingPage
    {
        [Display(
            GroupName = SystemTabNames.Content,
            Order = 350)]
        [CultureSpecific]
        public virtual ContentArea RightHandContentArea { get; set; }

        [Display(
            Name = "Column quantity of MainContentArea",
            GroupName = SystemTabNames.Content,
            Order = 400)]
        [CultureSpecific]
        public virtual int MainContentAreaColumn { get; set; }

        [Display(
            Name = "Column quantity of RightHandContentArea",
            GroupName = SystemTabNames.Content,
            Order = 450)]
        [CultureSpecific]
        public virtual int RightHandContentAreaColumn { get; set; }

        public override void SetDefaultValues(ContentType contentType)
        {
            this.MainContentAreaColumn = this.RightHandContentAreaColumn = 6;
            base.SetDefaultValues(contentType);
        }
    }


    public class TwoColumnLandingPageValidation : IValidate<TwoColumnLandingPage>
    {
        public IEnumerable<ValidationError> Validate(TwoColumnLandingPage instance)
        {
            var validations = new List<ValidationError>();
            if (instance.MainContentAreaColumn + instance.RightHandContentAreaColumn != 12)
            {
                var error = new ValidationError();
                error.PropertyName = nameof(instance.MainContentAreaColumn) + ", " + nameof(instance.RightHandContentAreaColumn);
                error.ErrorMessage = "Sum of columns must be 12. Properties " + error.PropertyName;
                error.Severity = ValidationErrorSeverity.Error;
                error.RelatedProperties = new List<string> { nameof(instance.MainContentAreaColumn), nameof(instance.RightHandContentAreaColumn) };
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

            return validations;
        }
    }
}
