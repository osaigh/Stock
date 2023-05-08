using System;
using System.Collections.Generic;
using System.Diagnostics;
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
        private int _port;
        private string _redirectUri;
        #endregion

        #region Properties

        #endregion

        #region Constructor

        public AuthenticationService()
        {
            _port = 44801;
            //browser = new BrowserProxy(port, 300);
            _redirectUri = string.Format($"http://127.0.0.1:{_port}/");
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

            var options = new OidcClientOptions
            {
                Authority = "https://localhost:44701",
                ClientId = "wpfclient",
                RedirectUri = _redirectUri,
                Scope = "openid profile email StockAPI offline_access",
                FilterClaims = false,
                TokenClientCredentialStyle = IdentityModel.Client.ClientCredentialStyle.AuthorizationHeader
                              //Flow = OidcClientOptions.AuthenticationFlow.AuthorizationCode
                              //Browser = browser,
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
                Process.Start(state.StartUrl);

                // wait for the authorization response.
                var context = await http.GetContextAsync();

                //get request data
                var data = GetRequestData(context.Request);

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
                    http.Stop();
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
                HttpListener http = null;
                try
                {
                    //http = GetHttpListener(_redirectUri);
                    var url = await _oidcClient.PrepareLogoutAsync(new LogoutRequest()
                    {
                        IdTokenHint = _result?.IdentityToken
                    });

                    // open system browser to logout
                    Process.Start(url);

                }catch (Exception ex)
                {
                    Debug.Write(ex);
                }
                finally
                {
                    if(http != null)
                    {
                        http.Stop();
                    }
                }

            }

            //if (browser != null)
            //{
            //    browser.KillProcess();
            //}
            
            _user = null;
        }

        /// <summary>
        /// Gets an HttpListener
        /// </summary>
        /// <param name="redirectUri"></param>
        /// <returns></returns>
        public HttpListener GetHttpListener(string redirectUri)
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
        public static string GetRequestData(HttpListenerRequest request)
        {
            //if (!request.HasEntityBody)
            //{
            //    return null;
            //}

            return request.RawUrl;

            using (var body = request.InputStream)
            {
                using (var reader = new System.IO.StreamReader(body, request.ContentEncoding))
                {
                    return reader.ReadToEnd();
                }
            }
        }
        #endregion
    }
}
