using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using MyLibrary.Validators;
using MyLibrary.ViewModels;

namespace MVCClient
{
    public static class Configuration
    {
        public static void UseServices(this IServiceCollection services)
        {
            services.AddTransient<IValidator<EstablishmentCreationVwMdl>, EstablishmentCreationValidator>();
            services.AddTransient<IValidator<EstablishmentEditionVwMdl>, EstablishmentEditionValidator>();
            services.AddTransient<IValidator<EstablishmentPicturesEditionVwMdl>, EstablishmentPicturesEditionValidator>();
        }

    }
}
