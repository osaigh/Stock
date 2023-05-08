using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Stock.WpfClient.Surveillance.Charts
{
    /// <summary>
    /// Interaction logic for LineChartControl.xaml
    /// </summary>
    public partial class LineChartControl : UserControl
    {
        #region Fields
        private ChartStyleGridlines cs;
        private DataCollection dc;
        private Legend lg;
        #endregion

        #region Properties
        #region ChartStyle
        public ChartStyleGridlines ChartStyle
        {
            get { return cs; }
            set { cs = value; }
        }
        #endregion

        #region XaxisValues
        public List<string> XaxisValues
        {
            get { return (List<string>)GetValue(XaxisValuesProperty); }
            set { SetValue(XaxisValuesProperty, value); }
        }

        // Using a DependencyProperty as the backing store for XaxisValues.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty XaxisValuesProperty =
            DependencyProperty.Register("XaxisValues", typeof(List<string>), typeof(LineChartControl), new FrameworkPropertyMetadata(new List<string>(), new PropertyChangedCallback(OnXaxisValuesPropertyChanged)));
        protected static void OnXaxisValuesPropertyChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            LineChartControl lineChartControl = sender as LineChartControl;
            lineChartControl?.UpdateChartStyleXAxisValues();
        }
        #endregion

        #region DataSeries
        public DataSeries DataSeries
        {
            get { return (DataSeries)GetValue(DataSeriesProperty); }
            set { SetValue(DataSeriesProperty, value); }
        }

        // Using a DependencyProperty as the backing store for DataSeries.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty DataSeriesProperty =
            DependencyProperty.Register("DataSeries", typeof(DataSeries), typeof(LineChartControl), new FrameworkPropertyMetadata(new DataSeries(), OnDataSeriesChanged));

        protected static void OnDataSeriesChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            LineChartControl lineChartControl = sender as LineChartControl;
            if(lineChartControl != null && e.NewValue != null)
            {
                lineChartControl.dc.DataList.Clear();
                lineChartControl.dc.DataList.Add(lineChartControl.DataSeries);
                lineChartControl.AddChart();
            }
        }
        #endregion

        #region Legend
        public Legend Legend
        {
            get { return lg; }
            set { lg = value; }
        }

        public static DependencyProperty XminProperty = DependencyProperty.Register("Xmin", typeof(double), typeof(LineChartControl),
            new FrameworkPropertyMetadata(0.0, new PropertyChangedCallback(OnPropertyChanged)));
        #endregion

        #region Xmin
        public double Xmin
        {
            get { return (double)GetValue(XminProperty); }
            set
            {
                SetValue(XminProperty, value);

            }
        }

        public static DependencyProperty XmaxProperty = DependencyProperty.Register("Xmax", typeof(double), typeof(LineChartControl),
            new FrameworkPropertyMetadata(10.0, new PropertyChangedCallback(OnPropertyChanged)));
        #endregion

        #region Xmax
        public double Xmax
        {
            get { return (double)GetValue(XmaxProperty); }
            set
            {
                SetValue(XmaxProperty, value);
            }
        }

        public static DependencyProperty YminProperty = DependencyProperty.Register("Ymin", typeof(double), typeof(LineChartControl),
            new FrameworkPropertyMetadata(0.0, new PropertyChangedCallback(OnPropertyChanged)));
        #endregion

        #region Ymin
        public double Ymin
        {
            get { return (double)GetValue(YminProperty); }
            set
            {
                SetValue(YminProperty, value);
            }
        }

        public static DependencyProperty YmaxProperty = DependencyProperty.Register("Ymax", typeof(double), typeof(LineChartControl),
            new FrameworkPropertyMetadata(10.0, new PropertyChangedCallback(OnPropertyChanged)));
        #endregion

        #region Ymax
        public double Ymax
        {
            get { return (double)GetValue(YmaxProperty); }
            set
            {
                SetValue(YmaxProperty, value);
            }
        }

        public static DependencyProperty XTickProperty = DependencyProperty.Register("XTick", typeof(double), typeof(LineChartControl),
            new FrameworkPropertyMetadata(2.0, new PropertyChangedCallback(OnPropertyChanged)));
        #endregion

        #region XTick
        public double XTick
        {
            get { return (double)GetValue(XTickProperty); }
            set
            {
                SetValue(XTickProperty, value);
            }
        }

        public static DependencyProperty YTickProperty = DependencyProperty.Register("YTick", typeof(double), typeof(LineChartControl),
            new FrameworkPropertyMetadata(2.0, new PropertyChangedCallback(OnPropertyChanged)));
        #endregion

        #region YTick
        public double YTick
        {
            get { return (double)GetValue(YTickProperty); }
            set
            {
                SetValue(YTickProperty, value);
            }
        }

        public static DependencyProperty XLabelProperty = DependencyProperty.Register("XLabel", typeof(string), typeof(LineChartControl),
            new FrameworkPropertyMetadata("Date", new PropertyChangedCallback(OnPropertyChanged)));
        #endregion

        #region XLabel
        public string XLabel
        {
            get { return (string)GetValue(XLabelProperty); }
            set
            {
                SetValue(XLabelProperty, value);
            }
        }

        public static DependencyProperty YLabelProperty = DependencyProperty.Register("YLabel", typeof(string), typeof(LineChartControl),
            new FrameworkPropertyMetadata("Price($)", new PropertyChangedCallback(OnPropertyChanged)));
        #endregion

        #region YLabel
        public string YLabel
        {
            get { return (string)GetValue(YLabelProperty); }
            set
            {
                SetValue(YLabelProperty, value);
            }
        }

        public static DependencyProperty TitleProperty = DependencyProperty.Register("Title", typeof(string), typeof(LineChartControl),
            new FrameworkPropertyMetadata("Title", new PropertyChangedCallback(OnPropertyChanged)));
        #endregion

        #region Title
        public string Title
        {
            get { return (string)GetValue(TitleProperty); }
            set
            {
                SetValue(TitleProperty, value);
            }
        }

        public static DependencyProperty IsXGridProperty = DependencyProperty.Register("IsXGrid", typeof(bool), typeof(LineChartControl),
            new FrameworkPropertyMetadata(true, new PropertyChangedCallback(OnPropertyChanged)));
        #endregion

        #region IsXGrid
        public bool IsXGrid
        {
            get { return (bool)GetValue(IsXGridProperty); }
            set
            {
                SetValue(IsXGridProperty, value);
            }
        }

        public static DependencyProperty IsYGridProperty = DependencyProperty.Register("IsYGrid", typeof(bool), typeof(LineChartControl),
           new FrameworkPropertyMetadata(true, new PropertyChangedCallback(OnPropertyChanged)));
        #endregion

        #region IsYGrid
        public bool IsYGrid
        {
            get { return (bool)GetValue(IsYGridProperty); }
            set
            {
                SetValue(IsYGridProperty, value);
            }
        }

        public static DependencyProperty GridlineColorProperty = DependencyProperty.Register("GridlineColor", typeof(Brush), typeof(LineChartControl),
           new FrameworkPropertyMetadata(Brushes.Gray, new PropertyChangedCallback(OnPropertyChanged)));
        #endregion

        #region GridlineColor
        public Brush GridlineColor
        {
            get { return (Brush)GetValue(GridlineColorProperty); }
            set
            {
                SetValue(GridlineColorProperty, value);
            }
        }

        public static DependencyProperty GridlinePatternProperty = DependencyProperty.Register("GridlinePattern",
            typeof(ChartStyleGridlines.GridlinePatternEnum), typeof(LineChartControl),
            new FrameworkPropertyMetadata(ChartStyleGridlines.GridlinePatternEnum.Solid,
            new PropertyChangedCallback(OnPropertyChanged)));
        #endregion

        #region GridlinePattern
        public ChartStyleGridlines.GridlinePatternEnum GridlinePattern
        {
            get { return (ChartStyleGridlines.GridlinePatternEnum)GetValue(GridlinePatternProperty); }
            set
            {
                SetValue(GridlinePatternProperty, value);
            }
        }

        public static DependencyProperty IsLegendProperty = DependencyProperty.Register("IsLegend", typeof(bool), typeof(LineChartControl),
            new FrameworkPropertyMetadata(false, new PropertyChangedCallback(OnPropertyChanged)));
        #endregion

        #region IsLegend
        public bool IsLegend
        {
            get { return (bool)GetValue(IsLegendProperty); }
            set
            {
                SetValue(IsLegendProperty, value);
            }
        }

        public static DependencyProperty LegendPositionProperty = DependencyProperty.Register("LegendPosition",
            typeof(Legend.LegendPositionEnum), typeof(LineChartControl),
            new FrameworkPropertyMetadata(Legend.LegendPositionEnum.NorthEast,
            new PropertyChangedCallback(OnPropertyChanged)));
        #endregion

        #region LegendPosition
        public Legend.LegendPositionEnum LegendPosition
        {
            get { return (Legend.LegendPositionEnum)GetValue(LegendPositionProperty); }
            set
            {
                SetValue(LegendPositionProperty, value);
            }
        }
        #endregion
        private static void OnPropertyChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            LineChartControl lcc = sender as LineChartControl;
            if (e.Property == XminProperty)
                lcc.cs.Xmin = (double)e.NewValue;
            else if (e.Property == XmaxProperty)
                lcc.cs.Xmax = (double)e.NewValue;
            else if (e.Property == YminProperty)
                lcc.cs.Ymin = (double)e.NewValue;
            else if (e.Property == YmaxProperty)
                lcc.cs.Ymax = (double)e.NewValue;
            else if (e.Property == XTickProperty)
                lcc.cs.XTick = (double)e.NewValue;
            else if (e.Property == YTickProperty)
                lcc.cs.YTick = (double)e.NewValue;
            else if (e.Property == GridlinePatternProperty)
                lcc.cs.GridlinePattern = (ChartStyleGridlines.GridlinePatternEnum)e.NewValue;
            else if (e.Property == GridlineColorProperty)
                lcc.GridlineColor = (Brush)e.NewValue;
            else if (e.Property == TitleProperty)
                lcc.cs.Title = (string)e.NewValue;
            else if (e.Property == XLabelProperty)
                lcc.cs.XLabel = (string)e.NewValue;
            else if (e.Property == YLabelProperty)
                lcc.cs.YLabel = (string)e.NewValue;
            else if (e.Property == IsXGridProperty)
                lcc.cs.IsXGrid = (bool)e.NewValue;
            else if (e.Property == IsYGridProperty)
                lcc.cs.IsYGrid = (bool)e.NewValue;
            else if (e.Property == IsLegendProperty)
                lcc.lg.IsLegend = (bool)e.NewValue;
            else if (e.Property == LegendPositionProperty)
                lcc.lg.LegendPosition = (Legend.LegendPositionEnum)e.NewValue;
        }
        #endregion

        #region Constructor
        public LineChartControl()
        {
            InitializeComponent();
            this.cs = new ChartStyleGridlines();
            this.dc = new DataCollection();
            this.lg = new Legend();
            cs.TextCanvas = textCanvas;
            cs.ChartCanvas = chartCanvas;
            lg.LegendCanvas = legendCanvas;
            grid1.DataContext = this;
        }
        #endregion

        #region Methods
        /// <summary>
        /// Update ChartStyle XAxisValues
        /// </summary>
        private void UpdateChartStyleXAxisValues()
        {
            this.cs.XAxisValues.Clear();
            if (this.XaxisValues != null && this.XaxisValues.Count > 0)
            {
                this.cs.XAxisValues.AddRange(this.XaxisValues);
                cs.Xmin = 0;
                if (this.XaxisValues.Count > 0)
                {
                    this.cs.Xmax = this.XaxisValues.Count - 1;
                }
            }
        }

        /// <summary>
        /// chartGrid_SizeChanged
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void chartGrid_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            textCanvas.Width = chartGrid.ActualWidth;
            textCanvas.Height = chartGrid.ActualHeight;
            
            AddChart();
        }
        
        /// <summary>
        /// Plots the chart
        /// </summary>
        private void AddChart()
        {
            legendCanvas.Children.Clear();
            chartCanvas.Children.RemoveRange(1, chartCanvas.Children.Count - 1);
            textCanvas.Children.RemoveRange(1, textCanvas.Children.Count - 1);
            
            if (this.DataSeries != null && this.DataSeries.Points.Count > 0)
            {
                double ymax = double.MinValue;
                double ymin = double.MaxValue;

                for(int i =0;i < this.DataSeries.Points.Count; i++)
                {
                    if(ymin > this.DataSeries.Points[i].Y)
                    {
                        ymin = this.DataSeries.Points[i].Y;
                    }

                    if (ymax < this.DataSeries.Points[i].Y)
                    {
                        ymax = this.DataSeries.Points[i].Y;
                    }
                }

                if(this.XaxisValues.Count > 0)
                {
                    this.cs.Xmax = this.XaxisValues.Count - 1;
                }

               
                cs.AddChartStyle(tbTitle, tbXLabel, tbYLabel);
                dc.AddLines(cs);
                Legend.AddLegend(chartCanvas, this.dc);
            }
            else
            {
                cs.AddChartStyle(tbTitle, tbXLabel, tbYLabel);
            }
        }

        #endregion
    }
}
