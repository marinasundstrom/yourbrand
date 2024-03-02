﻿using Duende.IdentityServer.Models;

using IdentityModel;

namespace YourBrand.IdentityService;

public static class Config
{
    public static IEnumerable<IdentityResource> IdentityResources =>
          new IdentityResource[]
          {
                new IdentityResources.OpenId(),

                // let's include the role claim in the profile
                new ProfileWithRoleIdentityResource(),
                new IdentityResources.Email()
          };

    public static IEnumerable<ApiResource> ApiResources =>
        new ApiResource[]
        {
                // the api requires the role claim
                new ApiResource("myapi", "The Web Api", new[] { JwtClaimTypes.Name, JwtClaimTypes.PreferredUserName, JwtClaimTypes.Email, JwtClaimTypes.Role })
                {
                    Scopes = new string[] { "myapi" }
                },
                // the api requires the role claim
                new ApiResource("worker", "The Web Api", new[] { JwtClaimTypes.Name, JwtClaimTypes.PreferredUserName, JwtClaimTypes.Email, JwtClaimTypes.Role })
                {
                    Scopes = new string[] { "myapi" }
                },
                // the api requires the role claim
                new ApiResource("timereport", "The Web Api", new[] { JwtClaimTypes.Name, JwtClaimTypes.PreferredUserName, JwtClaimTypes.Email, JwtClaimTypes.Role })
                {
                    Scopes = new string[] { "myapi" }
                }
        };

    public static IEnumerable<ApiScope> ApiScopes =>
        new ApiScope[]
        {
                    // the api requires the role claim
                    new ApiScope("myapi", "Access the api")
        };

    public static IEnumerable<Duende.IdentityServer.Models.Client> Clients =>
        new Duende.IdentityServer.Models.Client[]
        {
            new Duende.IdentityServer.Models.Client
            {
                ClientId = "blazor",
                AllowedGrantTypes = GrantTypes.Code,
                RequirePkce = true,
                RequireClientSecret = false,
                AllowedCorsOrigins = { "https://localhost:5174" },
                AllowedScopes = { "openid", "profile", "email", "myapi" },
                RedirectUris = { "https://localhost:5174/authentication/login-callback" },
                PostLogoutRedirectUris = { "https://localhost:5174/" },
                Enabled = true
            }
        };

}