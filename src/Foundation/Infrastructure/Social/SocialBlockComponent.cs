namespace Foundation.Social
{
    public abstract class SocialBlockComponent<T> : AsyncBlockComponent<T> where T : BlockData
    {
        protected readonly IPageRouteHelper _pageRouteHelper;

        protected SocialBlockComponent(IPageRouteHelper pageRouteHelper) => _pageRouteHelper = pageRouteHelper;

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