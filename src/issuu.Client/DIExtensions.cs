using issuu.Client;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;

namespace Microsoft.Extensions.DependencyInjection
{

    public static class DIExtensions
    {

        public static IServiceCollection AddIssuuClient(this IServiceCollection services, Action<IssuuOptions> configure = null)
        {
            services.AddTransient<IssuuClient>();
            services.AddOptions<IssuuOptions>("IssuuOptions");

            if(configure != null)
            {
                services.Configure<IssuuOptions>(configure);
            }

            return services;
        }

    }

}
