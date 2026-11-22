using Prism.Events;
using Prism.Mvvm;
using Stock.APIClient.API;
using Stock.WpfClient.Common.Events;
using Stock.WpfClient.Common.Services;
using Stock.WpfClient.Surveillance.Charts;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using DTOStockPosition = Stock.Models.StockPosition;
//using Stock.APIClient.API;

namespace Stock.WpfClient.Surveillance.ViewModels
{
    public class SurveillanceViewModel:BindableBase ,ISurveillanceViewModel
    {
        #region Fields
        protected readonly string _baseURL = "http://localhost:44700/api/";
        protected readonly string _identityServerUrl = "https://localhost:44701/";
        protected readonly string _clientId = "stock_api";
        protected readonly string _clientSecret = "dtkkb4ab34d9med859pgwrckvuvbhy";
        protected readonly string _scope = "StockAPI";
        protected StockAPIClient _stockApiClient ;
        protected readonly IEventAggregator _EventAggregator;
        protected readonly IAuthenticationService _AuthenticationService;
        protected CancellationTokenSource _marketUpdatesCancellation;
        //True while the list of positions is being replaced with fresh data, so the selection changes
        //caused by the refresh itself are not treated as the user selecting a stock
        private bool _isRefreshingStockPositions = false;
        #endregion

        #region Properties
        private string _chartTitle = string.Empty;
        public string ChartTitle
        {
            get { return _chartTitle; }
            set { SetProperty(ref _chartTitle, value); }
        }

        private Visibility _chartVisibility = Visibility.Collapsed;
        public Visibility ChartVisibility
        {
            get { return _chartVisibility; }
            set { SetProperty(ref _chartVisibility, value); }
        }

        private List<string> _xaxisValues = new List<string>();
        public List<string> XaxisValues
        {
            get { return _xaxisValues; }
            set { 
                SetProperty(ref _xaxisValues, value);
            }
        }

        private double _xmin = 0;
        public double Xmin
        {
            get { return _xmin; }
            set
            {
                SetProperty(ref this._xmin, value);
            }
        }

        private double _xmax = 10.0;
        public double Xmax
        {
            get { return _xmax; }
            set
            {
                SetProperty(ref this._xmax, value);
            }
        }

        private double _ymax = 10.0;
        public double Ymax
        {
            get { return _ymax; }
            set
            {
                SetProperty(ref this._ymax, value);
            }
        }

        private double _ymin = 1.0;
        public double Ymin
        {
            get { return _ymin; }
            set
            {
                SetProperty(ref this._ymin, value);
            }
        }

        private bool _isSignedIn = false;
        public bool IsSignedIn
        {
            get { return _isSignedIn; }
            set { SetProperty(ref this._isSignedIn, value); }
        }

        private ObservableCollection<DTOStockPosition> _stockPositions = new ObservableCollection<DTOStockPosition>();
        public ObservableCollection<DTOStockPosition> StockPositions
        {
            get { return _stockPositions; }
            set { SetProperty(ref this._stockPositions, value); }
        }

        private Visibility signedInVisibility = Visibility.Visible;
        public Visibility SignedInVisibility
        {
            get { return signedInVisibility; }
            set
            {
                SetProperty(ref this.signedInVisibility, value);
            }
        }

        private DataSeries _dataSeries = null;
        public DataSeries DataSeries
        {
            get { return _dataSeries; }
            set
            {
                SetProperty(ref this._dataSeries, value);
            }
        }

        private DTOStockPosition _selectedStock = null;
        public DTOStockPosition SelectedStock
        {
            get { return _selectedStock; }
            set
            {
                if (SetProperty(ref _selectedStock, value))
                {
                    if (_selectedStock != null && !_isRefreshingStockPositions)
                    {
                        OnSelectedStock();
                    }
                }
            }
        }
        #endregion

        #region Constructor
        public SurveillanceViewModel(IEventAggregator eventAggregator, IAuthenticationService authenticationService)
        {
            _EventAggregator = eventAggregator;
            _AuthenticationService = authenticationService;
            Initialize();
        }
        #endregion

        #region Methods
        private void Initialize()
        {
            
            //Register for events
            _EventAggregator.GetEvent<SignedInEvent>().Subscribe(OnSignedInEventHandler);
            _EventAggregator.GetEvent<SignedOutEvent>().Subscribe(OnSignedOutEventHandler);

            //Initialize Delegate
            _OnStockUpdateDelegate = new OnStockUpdateDelegate(OnStockUpdate);
        }

        protected void StartReceivingMarketUpdates()
        {
            StopReceivingMarketUpdates();

            //Create api client
            _stockApiClient = new StockAPIClient(_baseURL, _identityServerUrl, _clientId, _clientSecret, _scope);

            var cancellationTokenSource = new CancellationTokenSource();
            _marketUpdatesCancellation = cancellationTokenSource;
            Task.Run(() => ReceiveMarketUpdates(cancellationTokenSource.Token));
        }

        protected async Task ReceiveMarketUpdates(CancellationToken cancellationToken)
        {
            while (!cancellationToken.IsCancellationRequested)
            {
                try
                {
                    var user = _AuthenticationService.GetUserIdentity();
                    if (user != null)
                    {
                        var stockPositions = await _stockApiClient.GetStockPositions(user.AccessToken);
                        if (stockPositions != null && !cancellationToken.IsCancellationRequested)
                        {
                            Application.Current.Dispatcher.Invoke(_OnStockUpdateDelegate, stockPositions);
                        }
                    }
                }
                catch (Exception ex)
                {
                    //The API may be unavailable for a moment. Try again on the next update.
                    Debug.WriteLine(ex);
                }

                try
                {
                    await Task.Delay(3000, cancellationToken);
                }
                catch (OperationCanceledException)
                {
                    break;
                }
            }
        }

        protected void StopReceivingMarketUpdates()
        {
            _marketUpdatesCancellation?.Cancel();
            _marketUpdatesCancellation = null;
        }

        protected async void OnSelectedStock()
        {
            try
            {
                await ShowStockHistory(_selectedStock);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
            }
        }

        protected async Task ShowStockHistory(DTOStockPosition stockPosition)
        {
            var user = _AuthenticationService.GetUserIdentity();
            if (stockPosition == null || user == null)
            {
                return;
            }

            //get the stock Id
            string stockId = stockPosition.StockId;
            string name = stockPosition.StockId;
            
            //call the api to get the stockhistory data
            var stockApiClient = new StockAPIClient(_baseURL, _identityServerUrl, _clientId, _clientSecret, _scope);
            var stockHistoryCollection = await stockApiClient.GetStockHistory(stockId, user.AccessToken);
            if (stockHistoryCollection == null || stockHistoryCollection.Count == 0)
            {
                return;
            }

            this.ChartVisibility = Visibility.Visible;

            //create a dataseries for the chart
            DataSeries ds = new DataSeries();

            // Set the properties of the dataseries
            ds.LineColor = new SolidColorBrush(Color.FromRgb(59, 113, 202));
            ds.LineThickness = 1;
            ds.SeriesName = name;
            ds.Symbols.SymbolType = Symbols.SymbolTypeEnum.Circle;
            ds.Symbols.BorderColor = ds.LineColor;
            ds.Symbols.SymbolSize = 6;
            int index = 1;
            double ymax =double.MinValue;
            double ymin = double.MaxValue;
            List<string> dates = new List<string>();
            dates.Add(string.Empty);
            foreach (var stockHistory in stockHistoryCollection)
            {
                if(ymax < stockHistory.Price)
                {
                    ymax = stockHistory.Price;
                }
                if(ymin > stockHistory.Price)
                {
                    ymin = stockHistory.Price;
                }
                Point point = new Point(index++, stockHistory.Price);
                string date = stockHistory.Date.ToString("d");
                dates.Add(date);
                ds.Points.Add(point);
            }

            dates.Add(string.Empty);
            this.XaxisValues = dates;
            this.ChartTitle = name;
            this.Xmax = dates.Count;
            this.Ymax = ymax;
            this.Ymin = ymin;
            this.DataSeries = ds;

        }
        #endregion

        #region Events
        protected void OnSignedInEventHandler(object payload)
        {
            IsSignedIn = true;
            this.SignedInVisibility = Visibility.Visible;
            StartReceivingMarketUpdates();
        }

        protected void OnSignedOutEventHandler(object payload)
        {
            IsSignedIn = false;
            this.SignedInVisibility = Visibility.Collapsed;
            StopReceivingMarketUpdates();
        }
        #endregion

        #region Delegates
        protected delegate void OnStockUpdateDelegate(IEnumerable<DTOStockPosition> stocks);

        protected OnStockUpdateDelegate _OnStockUpdateDelegate;

        protected void OnStockUpdate(IEnumerable<DTOStockPosition> stockPositions)
        {
            //Replacing the items clears the selection in the view, so remember it and put it back afterwards
            string selectedStockId = _selectedStock?.StockId;
            _isRefreshingStockPositions = true;
            try
            {
                this.StockPositions.Clear();
                foreach (var stockPosition in stockPositions)
                {
                    var stockPositionModel = new DTOStockPosition()
                    {
                        StockId = stockPosition.StockId,
                        CurrentPrice = Math.Round(stockPosition.CurrentPrice,2),
                        CostBasis = Math.Round(stockPosition.CostBasis, 2),
                        Shares = stockPosition.Shares,
                    };
                    this.StockPositions.Add(stockPositionModel);
                }

                this.SelectedStock = selectedStockId == null ? null : this.StockPositions.FirstOrDefault(s => s.StockId == selectedStockId);
            }
            finally
            {
                _isRefreshingStockPositions = false;
            }
        }
        #endregion
    }
}
