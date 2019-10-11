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
        [Display(Name = "Left Hand Content Area", GroupName = SystemTabNames.Content, Order = 195)]
        public virtual ContentArea LeftHandContentArea { get; set; }

        [CultureSpecific]
        [Display(Name = "Right Hand Content Area", GroupName = SystemTabNames.Content, Order = 205)]
        public virtual ContentArea RightHandContentArea { get; set; }

        [CultureSpecific]
        [Display(Name = "Column Quantity of Main Content Area", GroupName = SystemTabNames.Content, Order = 210)]
        public virtual int MainContentAreaColumn { get; set; }

        [CultureSpecific]
        [Display(Name = "Column Quantity of Right Hand Content Area", GroupName = SystemTabNames.Content, Order = 211)]
        public virtual int RightHandContentAreaColumn { get; set; }

        [CultureSpecific]
        [Display(Name = "Column Quantity of Left Hand Content Area", GroupName = SystemTabNames.Content, Order = 212)]
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
