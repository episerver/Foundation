using EPiServer.Core;
using EPiServer.DataAbstraction;
using EPiServer.DataAnnotations;
using EPiServer.Validation;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Foundation.Cms.Pages
{
    [ContentType(
       GUID = "947EDF31-8C8C-4595-8591-A17DEF75685E",
       DisplayName = "Three Column Landing Page",
       Description = "Three column landing page with properties to determin column size.",
       GroupName = CmsTabs.Content)]
    [ImageUrl("~/assets/icons/gfx/page-type-thumbnail-landingpage-threecol.png")]
    public class ThreeColumnLandingPage : LandingPage
    {
        [Display(
            GroupName = SystemTabNames.Content,
            Order = 300)]
        [CultureSpecific]
        public virtual ContentArea LeftHandContentArea { get; set; }

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

        [Display(
            Name = "Column quantity of LeftHandContentArea",
            GroupName = SystemTabNames.Content,
            Order = 500)]
        [CultureSpecific]
        public virtual int LeftHandContentAreaColumn { get; set; }

        public override void SetDefaultValues(ContentType contentType)
        {
            this.MainContentAreaColumn = this.RightHandContentAreaColumn = this.LeftHandContentAreaColumn = 4;
            base.SetDefaultValues(contentType);
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
