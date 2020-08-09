using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using MyLibrary.Validators;
using MyLibrary.ViewModels;

namespace IdentityServer
{
    public static class MyConfiguration
    {
        public static void UseServices(this IServiceCollection services)
        {
            services.AddTransient<IValidator<RegisterVwMdl>, RegisterValidator>();
            services.AddTransient<IValidator<UserAccountEditionVwMdl>, UserAccountEditionValidator>();
        }

    }
}
