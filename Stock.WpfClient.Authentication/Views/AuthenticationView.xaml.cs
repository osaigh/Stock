using Stock.WpfClient.Authentication.ViewModels;
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

namespace Stock.WpfClient.Authentication.Views
{
    /// <summary>
    /// Interaction logic for AuthenticationView.xaml
    /// </summary>
    public partial class AuthenticationView : UserControl
    {
        #region Properties
        public IAuthenticationViewModel Model
        {
            get;set;
        }
        #endregion

        #region Constructor
        public AuthenticationView(IAuthenticationViewModel authenticationViewModel)
        {
            InitializeComponent();
            this.Model = authenticationViewModel;
            this.DataContext = authenticationViewModel;
        }
        #endregion
    }
}
