using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using TheBoringTeam.CIAssistant.BusinessEntities.Entities;
using TheBoringTeam.CIAssistant.BusinessLogic.Interfaces;

namespace TheBoringTeam.CIAssistant.API.Infrastructure
{
    public class BasicAuthenticationMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly IBaseBusinessLogic<User> _userBL;

        public BasicAuthenticationMiddleware(RequestDelegate next, IBaseBusinessLogic<User> userBL)
        {
            _next = next;
            _userBL = userBL;
        }

        public async Task Invoke(HttpContext context)
        {
            string authHeader = context.Request.Headers["Authorization"];
            if (authHeader != null && authHeader.StartsWith("Basic"))
            {
                string encodedUsernamePassword = authHeader.Substring("Basic ".Length).Trim();
                Encoding encoding = Encoding.GetEncoding("iso-8859-1");
                string usernamePassword = encoding.GetString(Convert.FromBase64String(encodedUsernamePassword));

                int seperatorIndex = usernamePassword.IndexOf(':');

                var username = usernamePassword.Substring(0, seperatorIndex);
                var password = usernamePassword.Substring(seperatorIndex + 1);

                using (var md5 = MD5.Create())
                {
                    byte[] array = md5.ComputeHash(Encoding.ASCII.GetBytes(password));
                    password = Encoding.ASCII.GetString(array);
                }

                User currentUser = _userBL.Search(f => f.Username == username && f.Password == password).FirstOrDefault();

                if (currentUser != null)
                {
                    ICollection<Claim> claims = new List<Claim>()
                    {
                        new Claim("UserId", currentUser.Id),
                        new Claim("AuthSchema", "Basic"),
                        new Claim("Roles", ((int)currentUser.Role).ToString())
                    };

                    ClaimsIdentity identity = new ClaimsIdentity(claims, "Basic");

                    var claimsPrincipal = new ClaimsPrincipal(new List<ClaimsIdentity>() { identity });
                    context.User = claimsPrincipal;
                }
            }

            await _next(context);
        }
    }

    public static class BasicAuthenticationMiddlewareExtensions
    {
        public static IApplicationBuilder UseBasicAuthenticationMiddleware(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<BasicAuthenticationMiddleware>();
        }
    }
}
