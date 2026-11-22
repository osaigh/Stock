using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Xunit;

namespace Stock.API.Tests.IntegrationTests
{
    public class AuthorizationIntegrationTests : IClassFixture<CustomWebApplicationFactory<Stock.API.Startup>>
    {
        #region Fields
        private readonly CustomWebApplicationFactory<Stock.API.Startup> _Factory;
        #endregion

        #region Constructor
        public AuthorizationIntegrationTests(CustomWebApplicationFactory<Stock.API.Startup> factory)
        {
            _Factory = factory;
        }
        #endregion

        #region Methods
        [Theory]
        [InlineData("api/stock")]
        [InlineData("api/stockhistory")]
        [InlineData("api/stockorder")]
        public async Task Get_WithoutAccessToken_ReturnsUnauthorized(string url)
        {
            //Arrange
            var client = _Factory.CreateClient();

            //Act
            var response = await client.GetAsync(url, TestContext.Current.CancellationToken);

            //Assert
            Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
        }

        [Fact]
        public async Task Get_WithInvalidAccessToken_ReturnsUnauthorized()
        {
            //Arrange
            var client = _Factory.CreateClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", "not-a-token");

            //Act
            var response = await client.GetAsync("api/stock", TestContext.Current.CancellationToken);

            //Assert
            Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
        }

        [Fact]
        public async Task Get_WithAccessTokenWithoutTheStockApiScope_ReturnsForbidden()
        {
            //Arrange
            var client = _Factory.CreateClient().WithAccessToken("openid", "profile");

            //Act
            var response = await client.GetAsync("api/stock", TestContext.Current.CancellationToken);

            //Assert
            Assert.Equal(HttpStatusCode.Forbidden, response.StatusCode);
        }

        [Fact]
        public async Task Get_WithAccessTokenWithTheStockApiScope_ReturnsOk()
        {
            //Arrange
            var client = _Factory.CreateClient().WithAccessToken("openid", TestTokens.StockApiScope);

            //Act
            var response = await client.GetAsync("api/stock", TestContext.Current.CancellationToken);

            //Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }

        [Fact]
        public async Task SwaggerDocument_IsAvailable()
        {
            //Arrange
            var client = _Factory.CreateClient();

            //Act
            var response = await client.GetAsync("swagger/v1/swagger.json", TestContext.Current.CancellationToken);

            //Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }
        #endregion
    }
}
