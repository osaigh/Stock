using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.JsonWebTokens;
using Microsoft.IdentityModel.Tokens;

namespace Stock.API.Tests
{
    /// <summary>
    /// Creates access tokens that look like the ones issued by Stock.IdentityServer, signed with a key
    /// that only the tests know. CustomWebApplicationFactory makes the API trust this key, so the
    /// integration tests run without an identity server.
    /// </summary>
    public static class TestTokens
    {
        public const string Issuer = "http://stock-api-tests";
        public const string StockApiScope = "StockAPI";

        public static readonly SymmetricSecurityKey SigningKey =
            new SymmetricSecurityKey(Encoding.UTF8.GetBytes("stock-api-tests-signing-key-that-is-long-enough-for-hs256"));

        public static string Create(params string[] scopes)
        {
            var claims = new List<Claim>() { new Claim("sub", "test-user") };
            claims.AddRange(scopes.Select(scope => new Claim("scope", scope)));

            return new JsonWebTokenHandler().CreateToken(new SecurityTokenDescriptor()
            {
                Issuer = Issuer,
                TokenType = "at+jwt",
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.UtcNow.AddMinutes(5),
                SigningCredentials = new SigningCredentials(SigningKey, SecurityAlgorithms.HmacSha256),
            });
        }

        /// <summary>
        /// Adds an access token with the given scopes to every request of the client
        /// </summary>
        public static HttpClient WithAccessToken(this HttpClient client, params string[] scopes)
        {
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Create(scopes));
            return client;
        }
    }
}
