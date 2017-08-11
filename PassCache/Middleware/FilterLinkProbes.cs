using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Primitives;

namespace PassCache.Middleware
{
    public class FilterLinkProbes
    {
        private static readonly string[] BlockedAgents = { "skype", "whatsapp" };
        private readonly RequestDelegate _next;

        public FilterLinkProbes(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            if (context.Request.Headers.TryGetValue("User-Agent", out StringValues userAgents))
            {
                if (userAgents.Select(a => a.ToLowerInvariant()).Any(userAgent => BlockedAgents.Any(blockedAgent => blockedAgent.Contains(userAgent))))
                {
                    context.Response.Redirect("/", true);
                    return;
                }
            }

            await _next(context);
        }
    }
}
