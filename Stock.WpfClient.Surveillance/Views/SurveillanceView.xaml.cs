using Stock.WpfClient.Surveillance.ViewModels;
using System;
using System.Collections.Generic;
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

namespace Stock.WpfClient.Surveillance.Views
{
    /// <summary>
    /// Interaction logic for SurveillanceView.xaml
    /// </summary>
    public partial class SurveillanceView : UserControl
    {
        #region Fields
        public ISurveillanceViewModel Model
        {
            get; set;
        }
        #endregion

        #region Constructor
        public SurveillanceView(ISurveillanceViewModel surveillanceViewModel)
        {
            InitializeComponent();
            this.Model = surveillanceViewModel;
            this.DataContext = Model;
        }

        #endregion
    }
}
