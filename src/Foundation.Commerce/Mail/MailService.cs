using EPiServer;
using EPiServer.Core;
using EPiServer.Web.Routing;
using Foundation.Cms.Pages;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Specialized;
using System.Net.Mail;
using System.Threading.Tasks;
using System.Web;

namespace Foundation.Commerce.Mail
{
    public class MailService : IMailService
    {
        private readonly IContentLoader _contentLoader;
        private readonly IHtmlDownloader _htmlDownloader;
        private readonly HttpContextBase _httpContextBase;
        private readonly UrlResolver _urlResolver;

        public MailService(HttpContextBase httpContextBase,
            UrlResolver urlResolver,
            IContentLoader contentLoader,
            IHtmlDownloader htmlDownloader)
        {
            _httpContextBase = httpContextBase;
            _urlResolver = urlResolver;
            _contentLoader = contentLoader;
            _htmlDownloader = htmlDownloader;
        }

        public void Send(ContentReference mailReference, NameValueCollection nameValueCollection, string toEmail,
            string language)
        {
            var body = GetHtmlBodyForMail(mailReference, nameValueCollection, language);
            var mailPage = _contentLoader.Get<MailBasePage>(mailReference);

            Send(mailPage.Subject, body, toEmail);
        }

        public string GetHtmlBodyForMail(ContentReference mailReference, NameValueCollection nameValueCollection,
            string language)
        {
            var urlBuilder = new UrlBuilder(_urlResolver.GetUrl(mailReference, language))
            {
                QueryCollection = nameValueCollection
            };

            var basePath = _httpContextBase.Request.Url.GetLeftPart(UriPartial.Authority);
            var relativePath = urlBuilder.ToString();

            if (relativePath.StartsWith(basePath))
            {
                relativePath = relativePath.Substring(basePath.Length);
            }

            return _htmlDownloader.Download(basePath, relativePath);
        }

        public void Send(string subject, string body, string recipientMailAddress)
        {
            var message = new MailMessage
            {
                Subject = subject,
                Body = body,
                IsBodyHtml = true
            };

            message.To.Add(recipientMailAddress);

            Send(message);
        }

        public void Send(MailMessage message)
        {
            using (var client = new SmtpClient())
            {
                // The SMTP host, port and sender e-mail address are configured
                // in the system.net section in web.config.
                client.Send(message);
            }
        }

        public Task SendAsync(IdentityMessage message)
        {
            Send(message.Subject, message.Body, message.Destination);
            return Task.FromResult(0);
        }
    }
}