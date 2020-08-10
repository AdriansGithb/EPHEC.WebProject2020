using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ExternalAPIs.Services;
using ExternalAPIs.Services.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace ExternalAPIs
{
    public static class Configuration
    {
        public static void UseServices(this IServiceCollection services)
        {
            services.AddHttpClient<IVatLayerService,VatLayerService>();
        }
    }
}
