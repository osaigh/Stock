using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using DTOStockPosition = Stock.Models.StockPosition;

namespace Stock.WpfClient.Surveillance.ViewModels
{
    public interface ISurveillanceViewModel
    {
        bool IsSignedIn { get; }

        string ChartTitle { get; }  

        ObservableCollection<DTOStockPosition> StockPositions { get; }

        Visibility SignedInVisibility { get; }

        Visibility ChartVisibility { get; }

        double Ymin { get; }
        double Ymax { get; }
        double Xmax { get; }
        double Xmin { get; }
        List<string> XaxisValues { get; }
    }
}
