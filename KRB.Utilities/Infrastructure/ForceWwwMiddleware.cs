
using System;
using System.Globalization;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Extensions;

namespace KRB.Utilities.Infrastructure;

public class ForceWwwMiddleware
{

    public async Task InvokeAsync(HttpContext context, RequestDelegate next)
    {
        if (context == null)
        {
            throw new ArgumentNullException(nameof(context));
        }

        if (context.Request.Host.Host.StartsWith("www"))
        {
            await next(context);
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

        await next(context);
    }



}

