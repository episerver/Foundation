﻿using Microsoft.AspNetCore.Mvc.Filters;

namespace Foundation.Infrastructure.Attributes
{
    public class OnlyAnonymousAttribute : ActionFilterAttribute
    {
        public override async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            await next();
            if (context.HttpContext.User.Identity.IsAuthenticated)
            {
                context.Result = new ForbidResult();
            }
        }
    }
}