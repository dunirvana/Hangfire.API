
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace Hangfire.API
{
    public static class ConfigurationHangfire
    {
        public static IServiceCollection ConfigureHangfire(this IServiceCollection services, IWebHostEnvironment hostingEnvironment, IConfiguration configuration)
        {
            if (hostingEnvironment == null) throw new ArgumentNullException(nameof(hostingEnvironment));
            if (configuration == null) throw new ArgumentNullException(nameof(configuration));


            return ConfigureForProduction(services, configuration);
        }


        private static IServiceCollection ConfigureForProduction(IServiceCollection services, IConfiguration configuration)
        {
            services.AddHangfire(op =>
            {
                op.UseSqlServerStorage(configuration.GetConnectionString("DefaultConnection"));

            });

            return services;
        }
    }
}
