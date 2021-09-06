using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using IdentityModel.Client;
using Newtonsoft.Json;
using Stock.APIClient.Errors;
using Stock.APIClient.Exceptions;

namespace Stock.APILibrary.API
{
    public class StockAPIBase
    {
        #region Fields
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
        protected async Task<T> FetchAsync<T>(string jsonString, string accessToken = null) where T:class
        {
            if (string.IsNullOrEmpty(accessToken))
            {
                accessToken = await GetAccessToken();
            }

            T returnValue = null;
            HttpResponseMessage responseMessage = null;

            using (var httpClient = new HttpClient())
            {
                httpClient.DefaultRequestHeaders.Accept.Clear();
                httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                httpClient.SetBearerToken(accessToken);
                responseMessage = await httpClient.GetAsync(new Uri(_baseURL + jsonString));

                if (responseMessage.IsSuccessStatusCode)
                {
                    var resultString = await responseMessage.Content.ReadAsStringAsync();
                    returnValue = JsonConvert.DeserializeObject<T>(resultString);
                }
            }

            return returnValue;
        }

        protected async Task<T> PostAsync<T>(string urlPostFix, string jsonIn, string accessToken = null) where T : class
        {
            T returnValue = null;
            HttpResponseMessage response = null;
            if (string.IsNullOrEmpty(accessToken))
            {
                accessToken = await GetAccessToken();
            }

            using (HttpClient client = new HttpClient())
            {
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                client.SetBearerToken(accessToken);
                response = await client.PostAsync(new Uri(_baseURL + urlPostFix), new StringContent(jsonIn, Encoding.UTF8, "application/json"));

                if (response.IsSuccessStatusCode)
                {
                    string jsonOut = await response.Content.ReadAsStringAsync();
                    returnValue = JsonConvert.DeserializeObject<T>(jsonOut);
                }
                else
                {
                    var exception = await GetException(response);
                    throw exception;

                }
            }

            return returnValue;
        }

        protected async Task PostAsync(string urlPostFix, string jsonIn, string accessToken = null)
        {
            HttpResponseMessage response = null;
            if (string.IsNullOrEmpty(accessToken))
            {
                accessToken = await GetAccessToken();
            }

            using (HttpClient client = new HttpClient())
            {
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                client.SetBearerToken(accessToken);
                response = await client.PostAsync(new Uri(_baseURL + urlPostFix), new StringContent(jsonIn, Encoding.UTF8, "application/json"));

                if (!response.IsSuccessStatusCode)
                {
                    var exception = await GetException(response);
                    throw exception;
                }
            }
        }

        protected async Task DeleteAsync(string urlPostFix, string accessToken = null)
        {
            HttpResponseMessage response = null;
            if (string.IsNullOrEmpty(accessToken))
            {
                accessToken = await GetAccessToken();
            }

            using (HttpClient client = new HttpClient())
            {
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                client.SetBearerToken(accessToken);
                response = await client.DeleteAsync(new Uri(_baseURL + urlPostFix));
                if (!response.IsSuccessStatusCode)
                {
                    var exception = await GetException(response);
                    throw exception;
                }
            }
        }

        protected async Task UpdateAsync(string urlPostFix, string jsonIn, string accessToken = null)
        {
            if (string.IsNullOrEmpty(accessToken))
            {
                //accessToken = await GetAccessToken();
            }

            HttpResponseMessage response = null;

            using (HttpClient client = new HttpClient())
            {
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                //client.SetBearerToken(accessToken);
                response = await client.PutAsync(new Uri(_baseURL + urlPostFix), new StringContent(jsonIn, Encoding.UTF8, "application/json"));
                if (!response.IsSuccessStatusCode)
                {
                    var exception = await GetException(response);
                    throw exception;
                }

            }
        }

        protected async Task<T> UpdateAsync<T>(string urlPostFix, string jsonIn, string accessToken = null) where T : class
        {
            HttpResponseMessage response = null;
            T returnValue = null;

            if (string.IsNullOrEmpty(accessToken))
            {
                accessToken = await GetAccessToken();
            }

            using (HttpClient client = new HttpClient())
            {
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                client.SetBearerToken(accessToken);
                response = await client.PutAsync(new Uri(_baseURL + urlPostFix), new StringContent(jsonIn, Encoding.UTF8, "application/json"));
                if (response.IsSuccessStatusCode)
                {
                    string json = await response.Content.ReadAsStringAsync();
                    returnValue = JsonConvert.DeserializeObject<T>(json);
                }
                else
                {
                    var exception = await GetException(response);
                    throw exception;
                }

            }

            return returnValue;
        }

        public async Task<string> GetAccessToken(string identityServerUrl, string clientId, string clientSecret, string scope)
        {
            TokenResponse tokenResponse = null;
            using (var httpClient = new HttpClient())
            {
                //Get discovery document
                DiscoveryDocumentResponse discoveryDocument = await httpClient.GetDiscoveryDocumentAsync(identityServerUrl);

                if (discoveryDocument.IsError)
                {
                    return string.Empty;
                }

                //get token response
                tokenResponse = await httpClient
                    .RequestClientCredentialsTokenAsync(
                        new ClientCredentialsTokenRequest()
                        {
                            Scope = scope,
                            ClientId = clientId,
                            ClientSecret = clientSecret,
                            Address = discoveryDocument.TokenEndpoint
                        });
            }

            return tokenResponse.IsError ? string.Empty : tokenResponse.AccessToken;
        }

        protected async Task<string> GetAccessToken()
        {
            return await GetAccessToken(_identityServerUrl, _clientId, _clientSecret, _scope);
        }

        protected async Task<Exception> GetException(HttpResponseMessage response)
        {
            Exception exception = null;
            if (response.StatusCode == HttpStatusCode.InternalServerError)
            {
                string json = await response.Content.ReadAsStringAsync();
                ErrorMessage error = JsonConvert.DeserializeObject<ErrorMessage>(json);
                exception = new StockAPIException(error.Message, response.RequestMessage.ToString(), error.StackTrace);
            }
            else
            {
                exception = new StockAPIException(response.ReasonPhrase, response.RequestMessage.ToString(), string.Empty);
            }

            return exception;
        }
        #endregion
    }
}
