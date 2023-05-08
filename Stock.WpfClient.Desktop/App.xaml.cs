using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Windows;
using Prism.Ioc;
using Prism.Modularity;
using Prism.Unity;
using Stock.WpfClient.Authentication;
using Stock.WpfClient.Common.Services;
using Stock.WpfClient.Desktop.ViewModels;
using Stock.WpfClient.Surveillance;

namespace Stock.WpfClient.Desktop
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : PrismApplication
    {
        protected override void ConfigureModuleCatalog(IModuleCatalog moduleCatalog)
        {
            moduleCatalog.AddModule<AuthenticationModule>();
            moduleCatalog.AddModule<SurveillanceModule>();
        }

        protected override Window CreateShell()
        {
            return (Window)(Container.Resolve(typeof(ShellWindow)));
        }

        protected override void RegisterTypes(IContainerRegistry containerRegistry)
        {
            containerRegistry.Register(typeof(IShellWindowViewModel), typeof(ShellWindowViewModel));
        }
    }
}
