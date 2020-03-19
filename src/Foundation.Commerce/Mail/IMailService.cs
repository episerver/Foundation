using EPiServer.Core;
using Microsoft.AspNet.Identity;
using System.Collections.Specialized;
using System.Net.Mail;
using System.Threading.Tasks;

namespace Foundation.Commerce.Mail
{
    public interface IMailService : IIdentityMessageService
    {
        void Send(string subject, string body, string toEmail);
        void Send(MailMessage message);
        Task SendAsync(ContentReference mailReference, NameValueCollection nameValueCollection, string toEmail, string language);
        Task SendAsync(MailMessage message);
        Task<string> GetHtmlBodyForMail(ContentReference mailReference, NameValueCollection nameValueCollection, string language);
    }
}