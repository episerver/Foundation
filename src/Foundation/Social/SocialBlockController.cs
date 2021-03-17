using EPiServer.Core;
using EPiServer.Web.Mvc;
using EPiServer.Web.Routing;
using System.Collections.Generic;
using System.Linq;

namespace Foundation.Social
{
    public abstract class SocialBlockController<T> : BlockController<T> where T : BlockData
    {
        protected readonly IPageRouteHelper _pageRouteHelper;

        protected SocialBlockController(IPageRouteHelper pageRouteHelper) => _pageRouteHelper = pageRouteHelper;

        public List<MessageViewModel> RetrieveMessages(string key)
        {
            var listOfMessages = (List<MessageViewModel>)TempData[key];

            return listOfMessages != null && listOfMessages.Any() ? listOfMessages : new List<MessageViewModel>();
        }

        public void AddMessage(string key, MessageViewModel value)
        {
            var listOfMessages = RetrieveMessages(key);
            listOfMessages.Add(value);
            TempData[key] = listOfMessages;
        }
    }
}