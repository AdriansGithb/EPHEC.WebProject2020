// Copyright (c) Brock Allen & Dominick Baier. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.


using System.Reflection;
using IdentityServer4;
using IdentityServer.Data;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using IdentityServerHost.Quickstart.UI;
using IdentityServer4.EntityFramework.DbContexts;
using System.Linq;
using IdentityServer4.EntityFramework.Mappers;
using System;
using System.Security.Claims;
using IdentityModel;
using MyLibrary.Constants;
using MyLibrary.Models;
using Serilog;

namespace IdentityServer
{
    public class Startup
    {
        public IWebHostEnvironment Environment { get; }
        public IConfiguration Configuration { get; }

        public Startup(IWebHostEnvironment environment, IConfiguration configuration)
        {
            Environment = environment;
            Configuration = configuration;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllersWithViews();

            var migrationsAssembly = typeof(Startup).GetTypeInfo().Assembly.GetName().Name;
            //const string ConnectionString = @"Data Source=MSI\SQLEXPRESS;database=WebProject2020;trusted_connection=yes;";

            services.AddDbContext<ApplicationDbContext>(ctxBuilder =>
                ctxBuilder.UseSqlServer(MyIdentityServerConstants.ConnectionString, sqlOptions => sqlOptions.MigrationsAssembly(migrationsAssembly)));

            services.AddIdentity<ApplicationUser, IdentityRole>()
                .AddEntityFrameworkStores<ApplicationDbContext>()
                .AddDefaultTokenProviders();

            var builder = services.AddIdentityServer(options =>
            {
                options.Events.RaiseErrorEvents = true;
                options.Events.RaiseInformationEvents = true;
                options.Events.RaiseFailureEvents = true;
                options.Events.RaiseSuccessEvents = true;

                // see https://identityserver4.readthedocs.io/en/latest/topics/resources.html
                options.EmitStaticAudienceClaim = true;
            })
                //.AddInMemoryIdentityResources(Config.IdentityResources)
                //.AddInMemoryApiScopes(Config.ApiScopes)
                //.AddInMemoryClients(Config.Clients)
                .AddAspNetIdentity<ApplicationUser>();

            builder.AddConfigurationStore(options =>
                {
                    options.ConfigureDbContext = b => b.UseSqlServer(MyIdentityServerConstants.ConnectionString,
                        sql => sql.MigrationsAssembly(migrationsAssembly));
                })
                .AddOperationalStore(options =>
                {
                    options.ConfigureDbContext = b => b.UseSqlServer(MyIdentityServerConstants.ConnectionString,
                        sql => sql.MigrationsAssembly(migrationsAssembly));
                });

            // not recommended for production - you need to store your key material somewhere secure
            builder.AddDeveloperSigningCredential();

        }

        public void Configure(IApplicationBuilder app)
        {
            if (Environment.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseDatabaseErrorPage();
            }

            //InitializeDatabase(app);

            app.UseStaticFiles();

            app.UseRouting();
            app.UseIdentityServer();
            app.UseAuthorization();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapDefaultControllerRoute();
            });
        }

        //private static void InitializeDatabase(IApplicationBuilder app)
        //{
        //    using (var serviceScope = app.ApplicationServices.GetService<IServiceScopeFactory>().CreateScope())
        //    {
        //        serviceScope.ServiceProvider.GetRequiredService<PersistedGrantDbContext>().Database.Migrate();
        //        serviceScope.ServiceProvider.GetRequiredService<ConfigurationDbContext>().Database.Migrate();
        //        serviceScope.ServiceProvider.GetRequiredService<ApplicationDbContext>().Database.Migrate();

        //        var context = serviceScope.ServiceProvider.GetRequiredService<ConfigurationDbContext>();

        //        if (!context.Clients.Any())
        //        {
        //            foreach (var client in Config.Clients)
        //            {
        //                context.Clients.Add(client.ToEntity());
        //            }

        //            context.SaveChanges();
        //        }

        //        if (!context.IdentityResources.Any())
        //        {
        //            foreach (var resource in Config.IdentityResources)
        //            {
        //                context.IdentityResources.Add(resource.ToEntity());
        //            }

        //            context.SaveChanges();
        //        }

        //        if (!context.ApiScopes.Any())
        //        {
        //            foreach (var scope in Config.ApiScopes)
        //            {
        //                context.ApiScopes.Add(scope.ToEntity());
        //            }

        //            context.SaveChanges();
        //        }


        //        var userMgr = serviceScope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();
        //        if (!userMgr.Users.Any())
        //        {
        //            var alice = new ApplicationUser
        //            {
        //                UserName = "alice",
        //                Email = "AliceSmith@email.com",
        //                EmailConfirmed = true,
        //            };
        //            var result = userMgr.CreateAsync(alice, "Pass123$").Result;
        //            if (!result.Succeeded)
        //            {
        //                throw new Exception(result.Errors.First().Description);
        //            }

        //            result = userMgr.AddClaimsAsync(alice, new Claim[]
        //            {
        //                new Claim(JwtClaimTypes.Name, "Alice Smith"),
        //                new Claim(JwtClaimTypes.GivenName, "Alice"),
        //                new Claim(JwtClaimTypes.FamilyName, "Smith"),
        //                new Claim(JwtClaimTypes.WebSite, "http://alice.com"),
        //                new Claim("role", MyIdentityServerConstants.Role_Manager), 
        //            }).Result;
        //            if (!result.Succeeded)
        //            {
        //                throw new Exception(result.Errors.First().Description);
        //            }

        //            Log.Debug("alice created");

        //            var bob = new ApplicationUser
        //            {
        //                UserName = "bob",
        //                Email = "BobSmith@email.com",
        //                EmailConfirmed = true
        //            };
        //            result = userMgr.CreateAsync(bob, "Pass123$").Result;
        //            if (!result.Succeeded)
        //            {
        //                throw new Exception(result.Errors.First().Description);
        //            }

        //            result = userMgr.AddClaimsAsync(bob, new Claim[]
        //            {
        //                new Claim(JwtClaimTypes.Name, "Bob Smith"),
        //                new Claim(JwtClaimTypes.GivenName, "Bob"),
        //                new Claim(JwtClaimTypes.FamilyName, "Smith"),
        //                new Claim(JwtClaimTypes.WebSite, "http://bob.com"),
        //                new Claim("location", "somewhere"),
        //                new Claim("role",MyIdentityServerConstants.Role_User), 
        //            }).Result;
        //            if (!result.Succeeded)
        //            {
        //                throw new Exception(result.Errors.First().Description);
        //            }

        //            Log.Debug("bob created");
        //        }
        //    }
        //}
    }
}