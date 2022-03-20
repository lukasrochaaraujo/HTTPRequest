using Microsoft.Extensions.DependencyInjection;
using System;

namespace HTTPRequest.DependencyInjection
{
    public static class HttpRequestDependencyInjection
    {
        public static void AddHttpRequest(this IServiceCollection services, Func<IServiceProvider, HttpRequest> options)
        {
            Func<IServiceProvider, HttpRequest> defaultOptions = (provider) =>
            {
                var httpOptions = new HttpRequestOptions
                {
                    IntevalBetweenAttemptsInSeconds = 3,
                    MaxRequestAttempts = 3,
                    TimeOutInSeconds = 60
                };

                return new HttpRequest(httpOptions);
            };

            services.AddTransient<IHttpRequest, HttpRequest>(options ?? defaultOptions);
        }
    }
}
