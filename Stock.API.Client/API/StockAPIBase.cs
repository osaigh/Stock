using System;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Duende.IdentityModel.Client;
using Newtonsoft.Json;
using Stock.APIClient.Errors;
using Stock.APIClient.Exceptions;

namespace Stock.APILibrary.API
{
    public class StockAPIBase
    {
        #region Fields
        // A single HttpClient is shared by all requests; creating one per request leaks sockets.
        private static readonly HttpClient _httpClient = new HttpClient();

        protected readonly string _baseURL = "";
        protected readonly string _identityServerUrl = "";
        protected readonly string _clientId = "";
        protected readonly string _clientSecret = "";
        protected readonly string _scope = "";
        #endregion

        #region Constructor
        public StockAPIBase(
            string baseUrl,
            string identityServerUrl,
            string clientId,
            string clientSecret,
            string scope
            )
        {
            _baseURL = baseUrl;
            _identityServerUrl = identityServerUrl;
            _clientId = clientId;
            _clientSecret = clientSecret;
            _scope = scope; 
        }
        #endregion

        #region Methods
        protected async Task<T> FetchAsync<T>(string urlPostFix, string accessToken = null) where T:class
        {
            if (string.IsNullOrEmpty(accessToken))
            {
                accessToken = await GetAccessToken();
            }

            T returnValue = null;

            using (HttpResponseMessage response = await SendAsync(HttpMethod.Get, urlPostFix, null, accessToken))
            {
                if (response.IsSuccessStatusCode)
                {
                    var resultString = await response.Content.ReadAsStringAsync();
                    returnValue = JsonConvert.DeserializeObject<T>(resultString);
                }
            }

            return returnValue;
        }

        protected async Task<T> PostAsync<T>(string urlPostFix, string jsonIn, string accessToken = null) where T : class
        {
            if (string.IsNullOrEmpty(accessToken))
            {
                accessToken = await GetAccessToken();
            }

            using (HttpResponseMessage response = await SendAsync(HttpMethod.Post, urlPostFix, jsonIn, accessToken))
            {
                if (!response.IsSuccessStatusCode)
                {
                    throw await GetException(response);
                }

                string jsonOut = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<T>(jsonOut);
            }
        }

        protected async Task PostAsync(string urlPostFix, string jsonIn, string accessToken = null)
        {
            if (string.IsNullOrEmpty(accessToken))
            {
                accessToken = await GetAccessToken();
            }

            using (HttpResponseMessage response = await SendAsync(HttpMethod.Post, urlPostFix, jsonIn, accessToken))
            {
                if (!response.IsSuccessStatusCode)
                {
                    throw await GetException(response);
                }
            }
        }

        protected async Task DeleteAsync(string urlPostFix, string accessToken = null)
        {
            if (string.IsNullOrEmpty(accessToken))
            {
                accessToken = await GetAccessToken();
            }

            using (HttpResponseMessage response = await SendAsync(HttpMethod.Delete, urlPostFix, null, accessToken))
            {
                if (!response.IsSuccessStatusCode)
                {
                    throw await GetException(response);
                }
            }
        }

        /// <summary>
        /// Sends a PUT request. No access token is requested when none is supplied,
        /// because the only caller (the market update endpoint) is anonymous.
        /// </summary>
        protected async Task UpdateAsync(string urlPostFix, string jsonIn, string accessToken = null)
        {
            using (HttpResponseMessage response = await SendAsync(HttpMethod.Put, urlPostFix, jsonIn, accessToken))
            {
                if (!response.IsSuccessStatusCode)
                {
                    throw await GetException(response);
                }
            }
        }

        protected async Task<T> UpdateAsync<T>(string urlPostFix, string jsonIn, string accessToken = null) where T : class
        {
            if (string.IsNullOrEmpty(accessToken))
            {
                accessToken = await GetAccessToken();
            }

            using (HttpResponseMessage response = await SendAsync(HttpMethod.Put, urlPostFix, jsonIn, accessToken))
            {
                if (!response.IsSuccessStatusCode)
                {
                    throw await GetException(response);
                }

                string json = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<T>(json);
            }
        }

        public async Task<string> GetAccessToken(string identityServerUrl, string clientId, string clientSecret, string scope)
        {
            //Get discovery document
            DiscoveryDocumentResponse discoveryDocument = await _httpClient.GetDiscoveryDocumentAsync(identityServerUrl);

            if (discoveryDocument.IsError)
            {
                return string.Empty;
            }

            //get token response
            TokenResponse tokenResponse = await _httpClient
                .RequestClientCredentialsTokenAsync(
                    new ClientCredentialsTokenRequest()
                    {
                        Scope = scope,
                        ClientId = clientId,
                        ClientSecret = clientSecret,
                        Address = discoveryDocument.TokenEndpoint
                    });

            return tokenResponse.IsError ? string.Empty : tokenResponse.AccessToken;
        }

        protected async Task<string> GetAccessToken()
        {
            return await GetAccessToken(_identityServerUrl, _clientId, _clientSecret, _scope);
        }

        protected async Task<Exception> GetException(HttpResponseMessage response)
        {
            string requestMessage = response.RequestMessage?.ToString() ?? string.Empty;

            if (response.StatusCode == HttpStatusCode.InternalServerError)
            {
                string json = await response.Content.ReadAsStringAsync();
                ErrorMessage error = TryDeserializeError(json);
                if (error != null)
                {
                    return new StockAPIException(error.Message, requestMessage, error.StackTrace);
                }
            }

            return new StockAPIException(response.ReasonPhrase, requestMessage, string.Empty);
        }

        private async Task<HttpResponseMessage> SendAsync(HttpMethod method, string urlPostFix, string jsonIn, string accessToken)
        {
            using (var request = new HttpRequestMessage(method, new Uri(_baseURL + urlPostFix)))
            {
                request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                if (!string.IsNullOrEmpty(accessToken))
                {
                    request.SetBearerToken(accessToken);
                }

                if (jsonIn != null)
                {
                    request.Content = new StringContent(jsonIn, Encoding.UTF8, "application/json");
                }

                return await _httpClient.SendAsync(request);
            }
        }

        private static ErrorMessage TryDeserializeError(string json)
        {
            try
            {
                return JsonConvert.DeserializeObject<ErrorMessage>(json);
            }
            catch (JsonException)
            {
                return null;
            }
        }
        #endregion
    }
}
