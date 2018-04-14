using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TheBoringTeam.CIAssistant.API.Infrastructure
{
    public class BasicAuthenticationAttribute : TypeFilterAttribute
    {
        public BasicAuthenticationAttribute() : base(typeof(BasicAuthenticationFilter))
        {
        }
    }

    public class BasicAuthenticationFilter : IAuthorizationFilter
    {
        public void OnAuthorization(AuthorizationFilterContext context)
        {
            var hasClaim = context.HttpContext.User.Claims.Any(c => c.Type == "AuthSchema" && c.Value == "Basic");
            if (!hasClaim)
            {
                context.Result = new UnauthorizedResult();
            }
        }
    }
}
