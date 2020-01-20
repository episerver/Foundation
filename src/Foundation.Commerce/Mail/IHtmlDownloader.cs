using System.Threading.Tasks;

namespace Foundation.Commerce.Mail
{
    public interface IHtmlDownloader
    {
        Task<string> Download(string baseUrl, string relativeUrl);
    }
}