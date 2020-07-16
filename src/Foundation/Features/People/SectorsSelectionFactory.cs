using EPiServer;
using EPiServer.Core;
using EPiServer.ServiceLocation;
using EPiServer.Shell.ObjectEditing;
using Foundation.Features.Home;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Foundation.Features.People
{
    public class SectorsSelectionFactory : ISelectionFactory
    {
        private static readonly Lazy<IContentLoader> _contentLoader = new Lazy<IContentLoader>(() => ServiceLocator.Current.GetInstance<IContentLoader>());

        public IEnumerable<ISelectItem> GetSelections(ExtendedMetadata metadata)
        {
            var startPage = _contentLoader.Value.Get<HomePage>(ContentReference.StartPage);
            return startPage.Sectors?.Select(x => new SelectItem { Value = x.Value, Text = x.Text }) ?? new List<SelectItem>();
        }
    }
}
