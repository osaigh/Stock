using Prism.Ioc;
using Prism.Modularity;
using Prism.Regions;
using Stock.WpfClient.Common;
using Stock.WpfClient.Surveillance.ViewModels;
using Stock.WpfClient.Surveillance.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Stock.WpfClient.Surveillance
{
    public class SurveillanceModule : IModule
    {
        public void OnInitialized(Prism.Ioc.IContainerProvider containerProvider)
        {
            var regionManager = (IRegionManager)containerProvider.Resolve(typeof(IRegionManager));
            //var container = containerProvider.R
            regionManager.AddToRegion(RegionNames.SurveillanceRegion, containerProvider.Resolve(typeof(SurveillanceView)));
   
        }

        public void RegisterTypes(Prism.Ioc.IContainerRegistry containerRegistry)
        {
            containerRegistry.Register(typeof(ISurveillanceViewModel), typeof(SurveillanceViewModel));
        }
    }
}
