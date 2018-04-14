using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TheBoringTeam.CIAssistant.BusinessEntities.Enums;

namespace TheBoringTeam.CIAssistant.API.Infrastructure
{
    public class RoleFilterAttribute : TypeFilterAttribute
    {
        public RoleFilterAttribute(RolesEnum role) : base(typeof(RoleFilter))
        {
            Arguments = new object[] { role };
        }
    }

    public class RoleFilter : IAuthorizationFilter
    {
        private readonly RolesEnum _role;

        public RoleFilter(RolesEnum role)
        {
            _role = role;
        }

        public void OnAuthorization(AuthorizationFilterContext context)
        {
            if (!context.HttpContext.User.Claims.Any(c => c.Type == "Role" && int.Parse(c.Value) > ((int)this._role)))
            {
                context.Result = new StatusCodeResult(403);
            }
        }
    }
}
