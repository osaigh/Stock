using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;

namespace Stock.WpfClient.Authentication.Services.Authentication
{
    public class LoopbackHttpListener : IDisposable
    {
        #region Fields
        private int DefaultTimeout = 300; 
        IWebHost _Host;
        TaskCompletionSource<string> _source = new TaskCompletionSource<string>();
        string _url;
        #endregion

        #region Properties
        public string Url => _url;
        #endregion

        #region Constructor

        public LoopbackHttpListener(int port, int defaultTimeout)
        {
            CancellationTokenSource source = new CancellationTokenSource();
            CancellationToken token = source.Token;
            DefaultTimeout = defaultTimeout;

            _url = $"http://127.0.0.1:{port}/";

            _Host = new WebHostBuilder()
                    .UseKestrel()
                    .UseUrls(_url)
                    .Configure(Configure)
                    .Build();
            _Host.StartAsync(token);
        }
        #endregion

        #region Methods
        void Configure(IApplicationBuilder app)
        {
            app.Run(async ctx =>
            {
                if (ctx.Request.Method == "GET")
                {
                    SetResult(ctx.Request.QueryString.Value, ctx);
                }
                else if (ctx.Request.Method == "POST")
                {
                    if (!ctx.Request.ContentType.Equals("application/x-www-form-urlencoded", StringComparison.OrdinalIgnoreCase))
                    {
                        ctx.Response.StatusCode = 415;
                    }
                    else
                    {
                        using (var sr = new StreamReader((Stream)ctx.Request.Body, Encoding.UTF8))
                        {
                            var body = await sr.ReadToEndAsync();
                            SetResult(body, ctx);
                        }
                    }
                }
                else
                {
                    ctx.Response.StatusCode = 405;
                }
            });
        }

        private void SetResult(string value, HttpContext ctx)
        {
            try
            {
                ctx.Response.StatusCode = 200;
                ctx.Response.ContentType = "text/html";
                ctx.Response.WriteAsync("<h3>You have successfully signed in. Please close the browser window and return back to the application</h3>");
                ctx.Response.Body.Flush();

                _source.TrySetResult(value);
            }
            catch
            {
                ctx.Response.StatusCode = 400;
                ctx.Response.ContentType = "text/html";
                ctx.Response.WriteAsync("<h1>Invalid request.</h1>");
                ctx.Response.Body.Flush();
            }
        }

        public Task<string> WaitForCallbackAsync()
        {
            Task.Run(async () =>
            {
                await Task.Delay(DefaultTimeout * 1000);
                _source.TrySetCanceled();
            });

            return _source.Task;
        }
        #endregion

        #region IDisposable
        public void Dispose()
        {
            Task.Run(async () =>
                     {
                         
                await Task.Delay(500);
                await _Host.StopAsync();
                var feat = _Host.ServerFeatures.First().Value;
                
                _Host.Dispose();
            });
        }
        #endregion
    }
}
