using Prism.Modularity;
using Prism.Regions;
using Stock.WpfClient.Authentication.ViewModels;
using Stock.WpfClient.Authentication.Views;
using Stock.WpfClient.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Stock.WpfClient.Authentication.Services;
using Stock.WpfClient.Common.Services;

namespace Stock.WpfClient.Authentication
{
    public class AuthenticationModule:IModule
    {
        public void OnInitialized(Prism.Ioc.IContainerProvider containerProvider)
        {
            var regionManager = (IRegionManager)containerProvider.Resolve(typeof(IRegionManager));
            //var container = containerProvider.R
            regionManager.AddToRegion(RegionNames.HeaderRegion, containerProvider.Resolve(typeof(AuthenticationView)));
        }

        public void RegisterTypes(Prism.Ioc.IContainerRegistry containerRegistry)
        {
            containerRegistry.Register(typeof(IAuthenticationViewModel),typeof(AuthenticationViewModel));
            containerRegistry.RegisterSingleton(typeof(IAuthenticationService), typeof(AuthenticationService));
        }
    }
}
