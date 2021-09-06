using System;
using System.Threading;
using Stock.APIClient.API;

namespace Stock.MarketUpdater
{
    public class Program
    {
        protected readonly string _baseURL = "https://localhost:44368/api/";
        protected readonly string _identityServerUrl = "https://localhost:44376/";
        protected readonly string _clientId = "stock_api";
        protected readonly string _clientSecret = "dtkkb4ab34d9med859pgwrckvuvbhy";
        protected readonly string _scope = "StockAPI";

        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");

            Program program = new Program();
            program.RunStart();
        }

        private void RunStart()
        {
            Thread thread = new Thread(UpdateMarket);
            thread.Start();
        }

        private void UpdateMarket()
        {
            StockAPIClient stockApiClient = new StockAPIClient(_baseURL,_identityServerUrl,_clientId,_clientSecret,_scope);

            while (true)
            {
                Thread.Sleep(3000);

                stockApiClient.UpdateStockMarket().GetAwaiter().GetResult();
            }
        }
    }
}
