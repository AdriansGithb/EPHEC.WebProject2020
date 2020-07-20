﻿// Copyright (c) Brock Allen & Dominick Baier. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.


using IdentityServer4.Models;
using MyConstants;
using System.Collections.Generic;
using IdentityServer4;

namespace IdentityServerAspNetIdentity
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

                    // where to redirect to after login
                    RedirectUris = { MyMVCConstants.MyMVC_OidcIn_Url },

                    // where to redirect to after logout
                    PostLogoutRedirectUris = { MyMVCConstants.MyMVC_OidcOut_Url },

                    AllowedScopes = new List<string>
                    {
                        IdentityServerConstants.StandardScopes.OpenId,
                        IdentityServerConstants.StandardScopes.Profile,
                        MyAPIConstants.MyAPI_Name
                    },

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
            };
    }
}