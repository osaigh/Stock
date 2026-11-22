using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Duende.IdentityServer;
using Duende.IdentityServer.Models;

namespace Stock.IdentityServer
{
    public static class Configuration
    {
        public static IEnumerable<ApiResource> GetApiResources()
        {
            return new List<ApiResource>(){new ApiResource("ApiOne"), new ApiResource("ApiTwo"), new ApiResource("StockAPI") };
        }

        public static IEnumerable<ApiScope> GetApiScopes()
        {
            return new List<ApiScope>() { new ApiScope("ApiOne"), new ApiScope("ApiTwo"), new ApiScope("StockAPI") };
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

            //The web clients are public clients (they cannot keep a secret), so they use the authorization code flow with PKCE
            return new List<Client>(){
                new Client()
                {
                    ClientId = "client_id_angular",
                    ClientName = "Stock Angular client",
                    RedirectUris = { "http://localhost:44902/signincallback" },
                    PostLogoutRedirectUris = { "http://localhost:44902/signoutcallback" },
                    AllowedGrantTypes = GrantTypes.Code,
                    AllowedScopes = {IdentityServerConstants.StandardScopes.OpenId, IdentityServerConstants.StandardScopes.Profile, IdentityServerConstants.StandardScopes.Email, "ApiOne","ApiTwo","StockAPI"},
                    AllowOfflineAccess = true,
                    RequireClientSecret = false,
                    RequireConsent = false,
                    AllowedCorsOrigins = { "http://localhost:44902" },
                    AccessTokenLifetime = 3600,
                },
                new Client(){
                    ClientId = "client_id_react",
                    ClientName = "Stock React client",
                    RedirectUris = { "http://localhost:44602/signincallback" },
                    PostLogoutRedirectUris = { "http://localhost:44602/signoutcallback" },
                    AllowedGrantTypes = GrantTypes.Code,
                    AllowedScopes = {IdentityServerConstants.StandardScopes.OpenId, IdentityServerConstants.StandardScopes.Profile, IdentityServerConstants.StandardScopes.Email, "ApiOne","ApiTwo","StockAPI"},
                    AllowOfflineAccess = true,
                    RequireClientSecret = false,
                    RequireConsent = false,
                    AllowedCorsOrigins = { "http://localhost:44602" },
                    AccessTokenLifetime = 3600,
                },
                new Client()
                {
                    ClientId = "wpfclient",
                    ClientName = "Stock WPF client",
                    RedirectUris = { "http://127.0.0.1:44801/" },
                    PostLogoutRedirectUris = { "http://127.0.0.1:44801/" },
                    AllowedGrantTypes = GrantTypes.Code,
                    AllowedScopes = {IdentityServerConstants.StandardScopes.OpenId, IdentityServerConstants.StandardScopes.Profile, IdentityServerConstants.StandardScopes.Email, "StockAPI"},
                    AllowOfflineAccess = true,
                    RequireClientSecret = false,
                    RequireConsent = false,
                    AllowedCorsOrigins = { "http://127.0.0.1:44801" },
                    AccessTokenLifetime = 3600,
                }

            };

        }

        /*****Open Id Configuration *******/
        /*
         * http://localhost:44702/.well-known/openid-configuration* 
         */
    }
}
