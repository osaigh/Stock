using Prism.Events;
using Prism.Mvvm;
using Stock.APIClient.API;
using Stock.WpfClient.Common.Events;
using Stock.WpfClient.Surveillance.Charts;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using DTOStock = Stock.Models.Stock;
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
        protected bool _keepRunning = false;
        object objLock = "lock";
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

        private ObservableCollection<DTOStock> _stocks = new ObservableCollection<DTOStock>();
        public ObservableCollection<DTOStock> Stocks
        {
            get { return _stocks; }
            set { SetProperty(ref this._stocks, value); }
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

        private DTOStock _selectedStock = null;
        public DTOStock SelectedStock
        {
            get { return _selectedStock; }
            set
            {
                if (SetProperty(ref _selectedStock, value))
                {
                    if (_selectedStock != null)
                    {
                        OnSelectedStock();
                    }
                }
            }
        }
        #endregion

        #region Constructor
        public SurveillanceViewModel(IEventAggregator eventAggregator)
        {
            _EventAggregator = eventAggregator;
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
            //Create api client
            _stockApiClient = new StockAPIClient(_baseURL, _identityServerUrl, _clientId, _clientSecret, _scope);

            _keepRunning = true;
            Task.Run(async () => {
                while (true)
                {
                    lock (objLock)
                    {
                        if (_keepRunning)
                        {
                            var stocks = _stockApiClient.GetStocks().GetAwaiter().GetResult();
                            Application.Current.Dispatcher.Invoke(_OnStockUpdateDelegate, stocks);
                        }
                        else
                        {
                            break;
                        }
                    }

                    await Task.Delay(3000);
                }
            });

        }

        protected void StopReceivingMarketUpdates()
        {
            lock (objLock)
            {
                _keepRunning = false;
            }
        }

        protected async Task OnSelectedStock()
        {
            this.ChartVisibility = Visibility.Visible;
            
            //get the stock Id
            int stockId = _selectedStock.Id;
            string name = _selectedStock.Name;
            
            //call the api to get the stockhistory data
            var stockApiClient = new StockAPIClient(_baseURL, _identityServerUrl, _clientId, _clientSecret, _scope);
            var stockHistoryCollection = await stockApiClient.GetStockHistory(stockId);

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
        protected delegate void OnStockUpdateDelegate(IEnumerable<DTOStock> stocks);

        protected OnStockUpdateDelegate _OnStockUpdateDelegate;

        protected void OnStockUpdate(IEnumerable<DTOStock> stocks)
        {
            this.Stocks.Clear();
            foreach (var stock in stocks)
            {
                var stockModel = new DTOStock()
                {
                    Id = stock.Id,
                    Price = Math.Round(stock.Price),
                    Volume = stock.Volume,
                    Name = stock.Name,
                };
                this.Stocks.Add(stockModel);
            }
            
            this.OnPropertyChanged(new System.ComponentModel.PropertyChangedEventArgs("Stocks"));
        }
        #endregion
    }
}
