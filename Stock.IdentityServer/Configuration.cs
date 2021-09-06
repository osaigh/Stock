using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IdentityModel;
using IdentityServer4;
using IdentityServer4.Models;

namespace Stock.IdentityServer
{
    public static class Configuration
    {
        public static IEnumerable<ApiResource> GetApis()
        {
            return new List<ApiResource>(){new ApiResource("ApiOne"), new ApiResource("ApiTwo"), new ApiResource("StockAPI") };
        }

        public static IEnumerable<IdentityResource> GetIdentityResources()
        {
            return new List<IdentityResource>()
                   {
                       new IdentityResources.OpenId(),
                       new IdentityResources.Email(),
                       new IdentityResources.Profile(),
                   };
        }

        public static IEnumerable<Client> GetClients()
        {
            //return new List<Client>(){new Client()
            //                          {
            //                              ClientId = "client_id",
            //                              ClientSecrets =  {new Secret("client_secret".ToSha256())},
            //                              AllowedGrantTypes = GrantTypes.ClientCredentials,
            //                              AllowedScopes = {"ApiOne"},
            //                              RequireConsent = false,
            //                          },
            //                             new Client()
            //                             {
            //                                 ClientId = "client_id_mvc",
            //                                 ClientSecrets =  {new Secret("client_secret_mvc".ToSha256())},
            //                                 AllowedGrantTypes = GrantTypes.Code,
            //                                 AllowedScopes = {"ApiOne","ApiTwo", IdentityServerConstants.StandardScopes.OpenId,IdentityServerConstants.StandardScopes.Email,IdentityServerConstants.StandardScopes.Profile},
            //                                 RedirectUris = { "https://localhost:44362/signin-oidc" },
            //                                 PostLogoutRedirectUris = { "https://localhost:44362/Home/Index" },
            //                                 RequireConsent = false,

            //                                 AllowOfflineAccess = true, //For refresh token
            //                                 //Puts all the claims in the Id token
            //                                 //AlwaysIncludeUserClaimsInIdToken = true,
            //                             },
            //                             new Client()
            //                             {
            //                                 ClientId = "client_id_js",
            //                                 RedirectUris = { "https://localhost:44374/home/signin" },
            //                                 PostLogoutRedirectUris = { "https://localhost:44374/Home/Index" },
            //                                 AllowedGrantTypes = GrantTypes.Implicit,
            //                                 AllowedScopes = {IdentityServerConstants.StandardScopes.OpenId,"ApiOne","ApiTwo"},
            //                                 AllowAccessTokensViaBrowser = true,
            //                                 RequireConsent = false,
            //                                 AllowedCorsOrigins = { "https://localhost:44374" },
            //                                 AccessTokenLifetime = 1,
            //                             },
            //                             new Client()
            //                             {
            //                                 ClientId = "stock_api",
            //                                 ClientName = "Stock API",
            //                                 ClientSecrets = {new Secret("dtkkb4ab34d9med859pgwrckvuvbhy".ToSha256()) },
            //                                 AllowedGrantTypes = GrantTypes.ClientCredentials,
            //                                 AllowedScopes = {"StockAPI"},
            //                                 RequireConsent = false,
            //                             }
            //                         };

            return new List<Client>(){
                                         new Client()
                                         {
                                             ClientId = "client_id_react",
                                             RedirectUris = { "http://localhost:3000/SignInCallback" },
                                             PostLogoutRedirectUris = { "http://localhost:3000/Home/" },
                                             AllowedGrantTypes = GrantTypes.Implicit,
                                             AllowedScopes = {IdentityServerConstants.StandardScopes.OpenId, IdentityServerConstants.StandardScopes.Profile, IdentityServerConstants.StandardScopes.Email, "ApiOne","ApiTwo","StockAPI"},
                                             AllowAccessTokensViaBrowser = true,
                                             RequireClientSecret = false,
                                             RequireConsent = false,
                                             AllowedCorsOrigins = { "http://localhost:3000" },
                                             AccessTokenLifetime = 1,
                                         },
                                         
                                     };
        }


        /*****Open Id Configuration *******/
        /*
         * https://localhost:44376/.well-known/openid-configuration* 
         */
    }
}
