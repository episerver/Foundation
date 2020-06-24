using EPiServer;
using EPiServer.Core;
using EPiServer.Web;
using EPiServer.Web.Routing;
using Foundation.Commerce.Customer;
using Mediachase.Commerce.Customers;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Foundation.Features.MyAccount.Bookmarks
{
    public interface IBookmarksService
    {
        void Add(Guid contentGuid);
        void Remove(Guid contentGuid);
        List<BookmarkModel> Get();
    }

    public class BookmarksService : IBookmarksService
    {
        private readonly IContentLoader _contentLoader;
        private readonly IUrlResolver _urlResolver;
        private readonly IPermanentLinkMapper _permanentLinkMapper;

        public BookmarksService(IContentLoader contentLoader,
            IUrlResolver urlResolver,
            IPermanentLinkMapper permanentLinkMapper)
        {
            _contentLoader = contentLoader;
            _urlResolver = urlResolver;
            _permanentLinkMapper = permanentLinkMapper;
        }

        public void Add(Guid contentGuid)
        {
            var currentContact = CustomerContext.Current.CurrentContact;
            if (currentContact != null)
            {
                var contentReference = _permanentLinkMapper.Find(contentGuid).ContentReference;
                var contact = new FoundationContact(currentContact);
                var bookmarkModel = new BookmarkModel();
                if (_contentLoader.TryGet<IContent>(contentReference, out var content))
                {
                    bookmarkModel.ContentLink = contentReference;
                    bookmarkModel.ContentGuid = content.ContentGuid;
                    bookmarkModel.Name = content.Name;
                    bookmarkModel.Url = _urlResolver.GetUrl(content);
                }

                var contactBookmarks = contact.ContactBookmarks;
                if (contactBookmarks.FirstOrDefault(x => x.ContentGuid == bookmarkModel.ContentGuid) == null)
                {
                    contactBookmarks.Add(bookmarkModel);
                }

                contact.Bookmarks = JsonConvert.SerializeObject(contactBookmarks);
                contact.SaveChanges();
            }
        }

        public List<BookmarkModel> Get()
        {
            var currentContact = CustomerContext.Current.CurrentContact;
            if (currentContact != null)
            {
                var contact = new FoundationContact(currentContact);
                return contact.ContactBookmarks;
            }

            return new List<BookmarkModel>();
        }

        public void Remove(Guid contentGuid)
        {
            var currentContact = CustomerContext.Current.CurrentContact;
            if (currentContact != null)
            {
                var contact = new FoundationContact(currentContact);
                var contactBookmarks = contact.ContactBookmarks;
                var content = contactBookmarks.FirstOrDefault(x => x.ContentGuid == contentGuid);
                contactBookmarks.Remove(content);
                contact.Bookmarks = contactBookmarks.Any() ? JsonConvert.SerializeObject(contactBookmarks) : "";
                contact.SaveChanges();
            }
        }
    }
}
