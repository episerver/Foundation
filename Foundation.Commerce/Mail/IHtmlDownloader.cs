namespace Foundation.Commerce.Mail
{
    public interface IHtmlDownloader
    {
        string Download(string baseUrl, string relativeUrl);
    }
}