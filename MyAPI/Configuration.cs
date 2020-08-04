using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using MyAPI.Services;
using MyAPI.Services.Interfaces;

namespace MyAPI
{
    public static class Configuration
    {
        public static void UseServices(this IServiceCollection services)
        {
            services.AddScoped<IUserAccountService, UserAccountService>();
        }
    }
}
