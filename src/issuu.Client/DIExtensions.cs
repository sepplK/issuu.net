using issuu.Client;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Extensions.Configuration;

namespace Microsoft.Extensions.DependencyInjection
{

    public static class DIExtensions
    {

        public static IServiceCollection AddIssuuClient(this IServiceCollection services)
        {
            services.AddTransient<IssuuClient>();
            services.AddOptions<IssuuOptions>("IssuuOptions");

            return services;
        }

    }

}
