using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.HttpOverrides;

namespace Api;

public static class ForwardedHeadersExtensions
{
    public static IApplicationBuilder UseForwardedHeaders(this IApplicationBuilder app)
    {
        var options = new ForwardedHeadersOptions
        {
            ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto
        };

        options.KnownNetworks.Clear();
        options.KnownProxies.Clear();

        return app.UseForwardedHeaders(options);
    }
}