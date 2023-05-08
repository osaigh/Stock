using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Stock.WpfClient.Surveillance.Charts
{
    public class DataCollection
    {
        #region Fields
        private List<DataSeries> dataList;
        #endregion

        #region Properties
        public List<DataSeries> DataList
        {
            get { return dataList; }
            set { dataList = value; }
        }
        #endregion

        #region Constructor
        public DataCollection()
        {
            dataList = new List<DataSeries>();
        }
        #endregion

        #region Methods
        public void AddLines(ChartStyle cs)
        {
            int j = 0;
            foreach (DataSeries ds in DataList)
            {
                if (ds.SeriesName == "Default Name")
                {
                    ds.SeriesName = "DataSeries" + j.ToString();
                }
                ds.AddLinePattern();
                ds.LineSeries.Points.Clear();
                for (int i = 0; i < ds.Points.Count; i++)
                {
                    ds.LineSeries.Points.Add(cs.NormalizePoint(ds.Points[i]));
                    ds.Symbols.AddSymbol(cs.ChartCanvas, ds.LineSeries.Points[i]);
                }
                cs.ChartCanvas.Children.Add(ds.LineSeries);
                j++;
            }
        }
        #endregion

    }
}
