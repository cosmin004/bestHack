using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using TheBoringTeam.CIAssistant.BusinessEntities.Entities;
using TheBoringTeam.CIAssistant.BusinessLogic.Interfaces;

namespace TheBoringTeam.CIAssistant.API.Infrastructure
{
    public class JWTBearerAuthenticationMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly IBaseBusinessLogic<User> _userBL;

        public JWTBearerAuthenticationMiddleware(RequestDelegate next, IBaseBusinessLogic<User> userBL)
        {
            _next = next;
            _userBL = userBL;
        }

        public async Task Invoke(HttpContext context, IConfiguration configuration)
        {
            string authHeader = context.Request.Headers["Authorization"];
            if (authHeader != null && authHeader.StartsWith("Bearer"))
            {
                try
                {
                    string token = authHeader.Substring("Bearer ".Length).Trim();

                    JwtSecurityTokenHandler handler = new JwtSecurityTokenHandler();
                    SecurityKey securityKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(configuration.GetValue<string>("appSecret")));
                    SecurityToken validatedToken;

                    TokenValidationParameters tokenParams = new TokenValidationParameters()
                    {
                        IssuerSigningKey = securityKey,
                        ValidIssuer = "IdentityServer",
                        ValidateAudience = false,
                        ValidateIssuer = true,
                        ValidateLifetime = true
                    };

                    ClaimsPrincipal principal = handler.ValidateToken(token, tokenParams, out validatedToken);

                    string id = principal.Claims.FirstOrDefault(f => f.Type == "UserId").Value;

                    if (id != null)
                    {
                        User currentUser = _userBL.Search(f => f.Id == id).FirstOrDefault();

                        if (currentUser != null)
                        {
                            ICollection<Claim> claims = new List<Claim>()
                            {
                                new Claim("UserId", currentUser.Id),
                                new Claim("AuthSchema", "Bearer"),
                                new Claim("Role", ((int)currentUser.Role).ToString())
                            };

                            ClaimsIdentity identity = new ClaimsIdentity(claims, "Basic");

                            var claimsPrincipal = new ClaimsPrincipal(new List<ClaimsIdentity>() { identity });
                            context.User = claimsPrincipal;
                        }
                    }

                }
                catch(Exception ex)
                {

                }
            }

            await _next(context);
        }
    }

    public static class JWTBearerAuthenticationMiddlewareExtensions
    {
        public static IApplicationBuilder UseTokenBasedAuthenticationMiddleware(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<JWTBearerAuthenticationMiddleware>();
        }
    }

}
