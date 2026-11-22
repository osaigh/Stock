using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Duende.IdentityModel.OidcClient;
using Stock.WpfClient.Common.Models;
using Stock.WpfClient.Common.Services;

namespace Stock.WpfClient.Authentication.Services
{
    public class AuthenticationService : IAuthenticationService
    {
        #region Fields
        private OidcClient _oidcClient;
        private User _user;
        private LoginResult _result;
        private int _port;
        private string _redirectUri;
        #endregion

        #region Properties

        #endregion

        #region Constructor

        public AuthenticationService()
        {
            _port = 44801;
            _redirectUri = string.Format($"http://127.0.0.1:{_port}/");
        }
        #endregion

        #region Methods
        protected void InitializeOidcClient()
        {

            var options = new OidcClientOptions
            {
                Authority = "http://localhost:44702",
                ClientId = "wpfclient",
                RedirectUri = _redirectUri,
                Scope = "openid profile email StockAPI offline_access",
                FilterClaims = false,
                TokenClientCredentialStyle = Duende.IdentityModel.Client.ClientCredentialStyle.AuthorizationHeader
            };



            _oidcClient = new OidcClient(options);
        }

        /// <summary>
        /// Signs the user in using Authorization Code flow
        /// </summary>
        /// <returns></returns>
        public async Task<SignInResult> SignIn()
        {
            if (_oidcClient == null)
            {
                InitializeOidcClient();
            }

            string error = null;
            _result = null;
            HttpListener http = null;
            try
            {
                http = GetHttpListener(_redirectUri);

                var state = await _oidcClient.PrepareLoginAsync();

                // open system browser to start authentication
                OpenBrowser(state.StartUrl);

                // wait for the authorization response.
                var context = await http.GetContextAsync();

                //get request data
                var data = GetRequestData(context.Request);

                //tell the user that the browser is no longer needed
                await SendResponseAsync(context.Response, "You have signed in. You can close this window and return to the Stock application.");

                //get authentication result
                _result = await _oidcClient.ProcessResponseAsync(data, state);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
                error = ex.Message;
            }
            finally
            {
                //stop the listener
                if (http != null)
                {
                    http.Close();
                }

                if(_result != null && _result.IsError)
                {
                    error = _result.Error;
                }
            }

            SignInResult signInResult = new SignInResult();

            //if we have an error
            if (!string.IsNullOrEmpty(error))
            {
                signInResult.HasError = true;
                signInResult.Errors = new List<string>();
                signInResult.Errors.Add(error);
            }
            else
            {
                //get claims from user
                if (_result.User != null)
                {
                    _user = new User();
                    _user.UserName = GetClaimValue(_result.User, "preferred_username", "name");
                    _user.FirstName = GetClaimValue(_result.User, "given_name");
                    _user.LastName = GetClaimValue(_result.User, "family_name");
                    _user.AccessToken = _result.AccessToken;
                    signInResult.User = _user;
                }
                else
                {
                    signInResult.HasError = true;
                    signInResult.Errors = new List<string>();
                    signInResult.Errors.Add("Unable to sign user in");
                }
            }

            return signInResult;
        }

        /// <summary>
        /// Signs the user out
        /// </summary>
        /// <returns></returns>
        public async Task SignOut()
        {
            _user = null;

            if (_oidcClient != null)
            {
                try
                {
                    var url = await _oidcClient.PrepareLogoutAsync(new LogoutRequest()
                    {
                        IdTokenHint = _result?.IdentityToken
                    });

                    // open system browser to logout
                    OpenBrowser(url);

                }catch (Exception ex)
                {
                    Debug.Write(ex);
                }
            }
        }

        /// <summary>
        /// Get User Identity
        /// </summary>
        /// <returns>User</returns>
        public User GetUserIdentity()
        {
            return _user;
        }

        /// <summary>
        /// Opens the url in the system browser
        /// </summary>
        /// <param name="url"></param>
        protected virtual void OpenBrowser(string url)
        {
            //UseShellExecute is required on .NET to open a url with the default browser
            Process.Start(new ProcessStartInfo(url) { UseShellExecute = true });
        }

        /// <summary>
        /// Gets an HttpListener
        /// </summary>
        /// <param name="redirectUri"></param>
        /// <returns></returns>
        private HttpListener GetHttpListener(string redirectUri)
        {
            var http = new HttpListener();
            http.Prefixes.Add(redirectUri);
            http.Start();
            return http;
        }

        /// <summary>
        /// Gets the data from the request
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        private static string GetRequestData(HttpListenerRequest request)
        {
            return request.RawUrl;
        }

        /// <summary>
        /// Writes a small html page to the browser
        /// </summary>
        private static async Task SendResponseAsync(HttpListenerResponse response, string message)
        {
            try
            {
                byte[] buffer = Encoding.UTF8.GetBytes($"<html><head><meta charset=\"utf-8\"><title>Stock WPF</title></head><body><h3>{WebUtility.HtmlEncode(message)}</h3></body></html>");
                response.ContentType = "text/html; charset=utf-8";
                response.ContentLength64 = buffer.Length;
                await response.OutputStream.WriteAsync(buffer, 0, buffer.Length);
                response.OutputStream.Close();
            }
            catch (Exception ex)
            {
                //The browser may already have closed the connection. This must not fail the sign in.
                Debug.WriteLine(ex);
            }
        }

        /// <summary>
        /// Gets the value of the first claim type that the user has
        /// </summary>
        private static string GetClaimValue(ClaimsPrincipal user, params string[] claimTypes)
        {
            foreach (string claimType in claimTypes)
            {
                var claim = user.FindFirst(claimType);
                if (claim != null && !string.IsNullOrEmpty(claim.Value))
                {
                    return claim.Value;
                }
            }

            return string.Empty;
        }
        #endregion
    }
}
