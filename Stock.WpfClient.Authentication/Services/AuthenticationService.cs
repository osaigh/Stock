using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using IdentityModel.OidcClient;
using Stock.WpfClient.Authentication.Services.Authentication;
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
        private BrowserProxy browser;
        #endregion

        #region Properties

        #endregion

        #region Constructor

        public AuthenticationService()
        {
            
        }
        #endregion

        #region Methods
        private int GetRandomUnusedPort()
        {
            var listener = new TcpListener(IPAddress.Loopback, 0);
            listener.Start();
            var port = ((IPEndPoint)listener.LocalEndpoint).Port;
            listener.Stop();
            return port;
        }


        protected void InitializeOidcClient()
        {
            // create a redirect URI using an available port on the loopback address.
            // requires the OP to allow random ports on 127.0.0.1 - otherwise set a static port
            int port = 44700;
            browser = new BrowserProxy(port, 300);
            string redirectUri = string.Format($"http://127.0.0.1:{port}/");

            var options = new OidcClientOptions
                          {
                              Authority = "https://localhost:44376",
                              ClientId = "wpfclient",
                              RedirectUri = redirectUri,
                              Scope = "openid profile email StockAPI offline_access",
                              FilterClaims = false,
                              Browser = browser,
                              Flow = OidcClientOptions.AuthenticationFlow.AuthorizationCode,
                              ResponseMode = OidcClientOptions.AuthorizeResponseMode.Redirect
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

            _result = null;
            try
            {
                _result = await _oidcClient.LoginAsync(new LoginRequest());
            }
            catch (Exception ex)
            {
                
            }
            SignInResult signInResult = new SignInResult();

            if (_result.IsError)
            {
                signInResult.HasError = true;
                signInResult.Errors = new List<string>();
                signInResult.Errors.Add(_result.Error);
            }

            //get claims from user
            if (_result.User != null)
            {
                _user = new User();
                var usernameClaim = _result.User.FindFirst(c => c.Type.ToLower() == "UserName".ToLower());
                if (usernameClaim != null && usernameClaim.Value != null)
                {
                    _user.UserName = usernameClaim.Value;
                }

                var firstNameClaim = _result.User.FindFirst(c => c.Type.ToLower() == "FirstName".ToLower());
                if (firstNameClaim != null && firstNameClaim.Value != null)
                {
                    _user.FirstName = firstNameClaim.Value;
                }

                var lastNameClaim = _result.User.FindFirst(c => c.Type.ToLower() == "LastName".ToLower());
                if (lastNameClaim != null && lastNameClaim.Value != null)
                {
                    _user.LastName = lastNameClaim.Value;
                }

                _user.AccessToken = _result.AccessToken;
                signInResult.User = _user;
            }
            else
            {
                signInResult.HasError = true;
                signInResult.Errors = new List<string>();
                signInResult.Errors.Add("Unable to sign user in");
            }

            return signInResult;
        }

        /// <summary>
        /// Signs the user out
        /// </summary>
        /// <returns></returns>
        public async Task SignOut()
        {
            if (_oidcClient != null)
            {
                await _oidcClient.LogoutAsync(new LogoutRequest()
                                              {
                                                IdTokenHint = _result?.IdentityToken
                });
                
            }

            if (browser != null)
            {
                browser.KillProcess();
            }
            
            _user = null;
        }
        #endregion
    }
}
