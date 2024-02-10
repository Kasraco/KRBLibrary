using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Extensions;
using System;
using System.Globalization;
using System.Threading.Tasks;

namespace KRB.Utilities.Infrastructure;

public class ForceWwwMiddleware
{
    private readonly RequestDelegate _next;

    public ForceWwwMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        if (context == null)
        {
            throw new ArgumentNullException(nameof(context));
        }

        if (context.Request.Host.Host.StartsWith("www"))
        {
            await _next(context);
            return;
        }

        var originalUri = context.Request.GetEncodedUrl();
        var newUri = new UriBuilder(originalUri)
        {
            Host = "www." + context.Request.Host.Host
        }.Uri;

        if (originalUri != newUri.AbsoluteUri)
        {
            context.Response.Redirect(newUri.AbsoluteUri, permanent: true);
            return;
        }

        await _next(context);
    }
}

