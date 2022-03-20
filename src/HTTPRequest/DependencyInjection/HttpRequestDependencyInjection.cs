using Microsoft.Extensions.DependencyInjection;
using System;

namespace HTTPRequest.DependencyInjection
{
    public static class HttpRequestDependencyInjection
    {
        public static IServiceCollection AddHttpRequest(this IServiceCollection services, Func<IServiceProvider, HttpRequest> options = null)
        {
            Func<IServiceProvider, HttpRequest> defaultOptions = (provider) 
                => new HttpRequest(new HttpRequestOptions());

            services.AddTransient<IHttpRequest, HttpRequest>(options ?? defaultOptions);

            return services;
        }
    }
}
