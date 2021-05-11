using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Foundation.Features.Api
{
    [Authorize]
    public class AuthorizedApiController : Controller
    {
    }
}