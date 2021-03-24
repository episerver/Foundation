namespace Foundation.Social.Models.Groups
{
    public class Community
    {
        public Community(string name, string description) : this("", name, description, "")
        {
        }

        public Community(string id, string name, string description) : this(id, name, description, "")
        {
            Id = id;
            Name = name;
            Description = description;
        }

        public Community(string id, string name, string description, string pageLink)
        {
            Id = id;
            Name = name;
            Description = description;
            PageLink = pageLink;
        }

        public string Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public string PageLink { get; set; }
    }
}