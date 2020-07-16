namespace Foundation.Social
{
    public class MessageViewModel
    {
        public MessageViewModel(string body, string type)
        {
            Body = body;
            Type = type;
        }

        public string Type { get; set; }

        public string Body { get; set; }

        public string ResolveStyle(string messageType)
        {
            switch (messageType)
            {
                case "Success":
                    return "green";
                case "Error":
                    return "red";
                default:
                    return "black";
            }
        }
    }
}