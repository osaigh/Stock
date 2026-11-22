using System;
using System.Threading;
using System.Threading.Tasks;
using Stock.APIClient.API;

namespace Stock.MarketUpdater
{
    public class Program
    {
        protected readonly string _baseURL = "http://localhost:44700/api/";
        protected readonly string _identityServerUrl = "http://localhost:44702/";
        protected readonly string _clientId = "stock_api";
        protected readonly string _clientSecret = "dtkkb4ab34d9med859pgwrckvuvbhy";
        protected readonly string _scope = "StockAPI";
        protected readonly TimeSpan _updateInterval = TimeSpan.FromSeconds(3);

        static async Task Main(string[] args)
        {
            using (var cancellationTokenSource = new CancellationTokenSource())
            {
                Console.CancelKeyPress += (sender, e) =>
                {
                    e.Cancel = true;
                    cancellationTokenSource.Cancel();
                };

                Program program = new Program();
                await program.UpdateMarket(cancellationTokenSource.Token);
            }
        }

        private async Task UpdateMarket(CancellationToken cancellationToken)
        {
            Console.WriteLine($"Updating stock prices at {_baseURL} every {_updateInterval.TotalSeconds} seconds. Press Ctrl+C to stop.");

            StockAPIClient stockApiClient = new StockAPIClient(_baseURL,_identityServerUrl,_clientId,_clientSecret,_scope);
            bool isConnected = true;

            while (!cancellationToken.IsCancellationRequested)
            {
                try
                {
                    await Task.Delay(_updateInterval, cancellationToken);
                    await stockApiClient.UpdateStockMarket();

                    if (!isConnected)
                    {
                        Console.WriteLine("Connected to the Stock API again.");
                        isConnected = true;
                    }
                }
                catch (OperationCanceledException) when (cancellationToken.IsCancellationRequested)
                {
                    break;
                }
                catch (Exception ex)
                {
                    //The API may not be running yet (or may have been restarted). Keep trying.
                    if (isConnected)
                    {
                        Console.WriteLine($"Could not update the stock market ({ex.Message}). Retrying every {_updateInterval.TotalSeconds} seconds...");
                        isConnected = false;
                    }
                }
            }
        }
    }
}
