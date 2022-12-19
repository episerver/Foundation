using EPiServer.Core;
using EPiServer.DataAbstraction;
using System.Threading.Tasks;
using EPiServer.Web.Routing;
using Foundation.Features.Login;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authorization.Policy;
using Microsoft.AspNetCore.Http;
using System.Linq;

namespace Foundation.Infrastructure.Cms.Users;

public class LoginRedirectAuthorizationMiddlewareResultHandler : IAuthorizationMiddlewareResultHandler
{
    private readonly AuthorizationMiddlewareResultHandler defaultHandler = new();

    private readonly IUrlResolver _urlResolver;
    private readonly IContentTypeRepository _contentTypeRepository;
    private readonly IContentModelUsage _contentModelUsage;

    public LoginRedirectAuthorizationMiddlewareResultHandler(IUrlResolver urlResolver, IContentTypeRepository contentTypeRepository, IContentModelUsage contentModelUsage)
    {
        _urlResolver = urlResolver;
        _contentTypeRepository = contentTypeRepository;
        _contentModelUsage = contentModelUsage;
    }

    public async Task HandleAsync(RequestDelegate next, HttpContext context, AuthorizationPolicy policy, PolicyAuthorizationResult authorizeResult)
    {
        var isEpiserverAdminUrl = context.Request.Path.StartsWithSegments(new PathString("/episerver"));
        if (authorizeResult.Challenged && !isEpiserverAdminUrl)
        {
            var contentType = _contentTypeRepository.Load<LoginPage>();
            var usages = _contentModelUsage.ListContentOfContentType(contentType);

            var page = usages.First();
            
            var url = _urlResolver.GetUrl(page.ContentLink);
            context.Response.Redirect(url);

            return;
        }

        await defaultHandler.HandleAsync(next, context, policy, authorizeResult);
    }
}