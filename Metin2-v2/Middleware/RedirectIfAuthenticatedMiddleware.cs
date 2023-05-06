using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;

namespace Metin2_v2.Middleware
{
    public class RedirectIfAuthenticatedMiddleware
    {
        private readonly RequestDelegate _next;

        public RedirectIfAuthenticatedMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            if (context.Request.Path.Value.StartsWith("/Account/Login") && context.User.Identity.IsAuthenticated)
            {
                context.Response.Redirect("/Player/Index");
            }
            else
            {
                await _next(context);
            }
        }
    }
}
