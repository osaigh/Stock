using System;
using System.Collections.Generic;
using System.Text;
using Stock.APILibrary.API;

namespace Stock.APIClient.API
{
    public partial class StockAPIClient : StockAPIBase
    {

        #region Constructor

        public StockAPIClient(
            string baseUrl,
            string identityServerUrl,
            string clientId,
            string clientSecret,
            string scope):base(baseUrl, identityServerUrl, clientId, clientSecret, scope)
        {

        }
        #endregion`
    }
}
