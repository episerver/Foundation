using System.ComponentModel.DataAnnotations;
using EPiServer.Core;
using EPiServer.DataAbstraction;
using EPiServer.DataAnnotations;
using EPiServer.Web;
using EPiServer.Web.Routing;

namespace Geta.EpiCategories
{
    [AvailableContentTypes(Availability = Availability.Specific, Include = new[] { typeof(CategoryData) })]
    public class CategoryData : StandardContentBase, IRoutable
    {
        private string _routeSegment;
        private bool _isModified;

        [UIHint(UIHint.PreviewableText)]
        [CultureSpecific]
        public virtual string RouteSegment
        {
            get { return _routeSegment; }
            set
            {
                ThrowIfReadOnly();
                _isModified = true;
                _routeSegment = value;
            }
        }

        [Display(Order = 20)]
        [UIHint(UIHint.Textarea)]
        [CultureSpecific]
        public virtual string Description { get; set; }

        [Display(Order = 30)]
        [CultureSpecific]
        public virtual bool IsSelectable { get; set; }

        protected override bool IsModified
        {
            get
            {
                if (base.IsModified == false)
                {
                    return _isModified;
                }

                return true;
            }
        }

        public override void SetDefaultValues(ContentType contentType)
        {
            base.SetDefaultValues(contentType);
            IsSelectable = true;
        }
    }
}