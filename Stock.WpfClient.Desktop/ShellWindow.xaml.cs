
using Stock.WpfClient.Desktop.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Ribbon;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Stock.WpfClient.Desktop
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class ShellWindow : RibbonWindow
    {
        #region fields

        #endregion

        #region Properties
        public IShellWindowViewModel Model
        {
            get;set;
        }


        //SignedIn DP
        public bool SignedIn
        {
            get { return (bool)GetValue(SignedInProperty); }
            set { SetValue(SignedInProperty, value); }
        }

        // Using a DependencyProperty as the backing store for SignedIn.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty SignedInProperty =
            DependencyProperty.Register("SignedIn", typeof(bool), typeof(ShellWindow), new FrameworkPropertyMetadata(false, OnSignedInPropertyChanged));

        protected static void OnSignedInPropertyChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            ShellWindow window = sender as ShellWindow;
            if (window.SignedIn)
            {
                window?.RaiseContentLoadedEvent();
                window.surveillance.Visibility = Visibility.Visible;
            }
            else
            {
                window?.RaiseContentUnLoadedEvent();
                window.surveillance.Visibility = Visibility.Collapsed;
            }
        }


        //Routed Event ContentLoaded
        public static readonly RoutedEvent ContentLoadedEvent = EventManager.RegisterRoutedEvent(
            "ContentLoaded", RoutingStrategy.Direct, typeof(RoutedEventHandler), typeof(ShellWindow));
        
        public event RoutedEventHandler ContentLoaded
        {
            add { AddHandler(ContentLoadedEvent, value); }
            remove { RemoveHandler(ContentLoadedEvent, value); }
        }

        //Routed Event ContentUnLoaded
        public static readonly RoutedEvent ContentUnLoadedEvent = EventManager.RegisterRoutedEvent(
            "ContentUnLoaded", RoutingStrategy.Direct, typeof(RoutedEventHandler), typeof(ShellWindow));

        public event RoutedEventHandler ContentUnLoaded
        {
            add { AddHandler(ContentUnLoadedEvent, value); }
            remove { RemoveHandler(ContentUnLoadedEvent, value); }
        }
        #endregion

        #region Constructor
        /// <summary>
        /// ShellWindow Constructor
        /// </summary>
        public ShellWindow(IShellWindowViewModel shellWindowViewModel)
        {
            InitializeComponent();
            this.Model = shellWindowViewModel;

            Binding shellWindowBinding = new Binding("SignedIn");
            shellWindowBinding.Source = shellWindowViewModel;
            this.SetBinding(SignedInProperty, shellWindowBinding);
        }
        #endregion

        #region Methods
        /// <summary>
        /// This method raises the ContentLoaded event
        /// </summary>
        private void RaiseContentLoadedEvent()
        {
            var args = new RoutedEventArgs(ContentLoadedEvent);
            RaiseEvent(args);
        }

        /// <summary>
        /// This method raises the ShellUnLoaded event
        /// </summary>
        private void RaiseContentUnLoadedEvent()
        {
            var args = new RoutedEventArgs(ContentUnLoadedEvent);
            RaiseEvent(args);
        }
        #endregion

    }
}
