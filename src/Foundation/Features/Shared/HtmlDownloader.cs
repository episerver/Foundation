using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

namespace Foundation.Features.Shared
{
    public interface IHtmlDownloader
    {
        Task<string> Download(string baseUrl, string relativeUrl);
    }

    public class HtmlDownloader : IHtmlDownloader
    {
        public async Task<string> Download(string baseUrl, string relativeUrl)
        {
            var client = new HttpClient { BaseAddress = new Uri(baseUrl) };
            var fullUrl = client.BaseAddress + relativeUrl;

            var response = await client.GetAsync(fullUrl);
            if (response.StatusCode == HttpStatusCode.NotFound)
            {
                return null;
            }

            if (!response.IsSuccessStatusCode)
            {
                throw new HttpRequestException(
                    string.Format("Request to '{0}' was unsuccessful. Content:\n{1}",
                        fullUrl, response.Content.ReadAsStringAsync().Result));
            }

            return await response.Content.ReadAsStringAsync();
        }
    }
}