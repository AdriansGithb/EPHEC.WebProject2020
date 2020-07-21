// Copyright (c) Brock Allen & Dominick Baier. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.


using IdentityServer4;
using IdentityServer4.Models;
using System.Collections.Generic;
using MyLibrary.Constants;

namespace IdentityServer
{
    public static class Config
    {
        public static IEnumerable<ApiScope> ApiScopes =>
            new List<ApiScope>
            {
                new ApiScope(MyAPIConstants.MyAPI_Name)
            };

        public static IEnumerable<Client> Clients =>
            new List<Client>
            {
                new Client
                {
                    //machine to machine client
                    ClientId = "client",

                    // no interactive user, use the clientid/secret for authentication
                    AllowedGrantTypes = GrantTypes.ClientCredentials,

                    // secret for authentication
                    ClientSecrets =
                    {
                        new Secret("secret".Sha256())
                    },

                    // scopes that client has access to
                    AllowedScopes = { MyAPIConstants.MyAPI_Name }
                },
                // interactive ASP.NET Core MVC client
                new Client
                {
                    ClientId = MyMVCConstants.MyMVC_Name,
                    ClientSecrets = { new Secret("secret".Sha256()) },

                    AllowedGrantTypes = GrantTypes.Code,
                    RequireConsent = false,

                    // where to redirect to after login
                    RedirectUris = { MyMVCConstants.MyMVC_OidcIn_Url },

                    // where to redirect to after logout
                    PostLogoutRedirectUris = { MyMVCConstants.MyMVC_OidcOut_Url },

                    AllowedScopes = new List<string>
                    {
                        IdentityServerConstants.StandardScopes.OpenId,
                        IdentityServerConstants.StandardScopes.Profile,
                        MyAPIConstants.MyAPI_Name,
                        "role"
                    },

                    AlwaysIncludeUserClaimsInIdToken = true,

                    //enable support for refresh tokens
                    AllowOfflineAccess = true
                }

            };

        //Add support for the standard openid(subject id) and profile(first name, last name etc..) scopes by ammending the IdentityResources property
        public static IEnumerable<IdentityResource> IdentityResources =>
            new List<IdentityResource>
            {
                new IdentityResources.OpenId(),
                new IdentityResources.Profile(),
                new IdentityResource("role", new []{"role"})
            };
    }
}