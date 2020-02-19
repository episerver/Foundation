using EPiServer.Core;
using EPiServer.SpecializedProperties;
using System;
using System.ComponentModel.DataAnnotations;

namespace Foundation.Cms.Attributes
{
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
    public class MaxElementsAttribute : ValidationAttribute
    {
        private readonly int _maxItemAllowed;

        public MaxElementsAttribute(int maxItemAllowed)
        {
            _maxItemAllowed = maxItemAllowed;
        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value == null)
            {
                return null;
            }
            if (value is LinkItemCollection && ((LinkItemCollection)value).Count > _maxItemAllowed)
            {
                return new ValidationResult($"Link Item Colleciton exceeds the maximum limit of {_maxItemAllowed} item(s)");
            }
            if (value is ContentArea && ((ContentArea)value).Count > _maxItemAllowed)
            {
                return new ValidationResult($"Content Area exceeds the maximum limit of {_maxItemAllowed} item(s)");
            }

            return null;
        }
    }
}
