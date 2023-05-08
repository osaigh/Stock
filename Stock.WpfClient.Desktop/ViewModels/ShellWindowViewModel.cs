using Prism.Events;
using Prism.Mvvm;
using Stock.WpfClient.Common.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Stock.WpfClient.Desktop.ViewModels
{
    public class ShellWindowViewModel:BindableBase, IShellWindowViewModel
    {
        #region Fields

        #endregion

        #region Properties
        private bool _signedIn = false;
        public bool SignedIn
        {
            get { return _signedIn; }
            set
            {
                SetProperty(ref _signedIn, value);
            }
        }
        #endregion

        #region Constructor
        public ShellWindowViewModel(IEventAggregator evenAggregator)
        {
            evenAggregator.GetEvent<SignedInEvent>().Subscribe(this.OnSignedInEvent);
            evenAggregator.GetEvent<SignedOutEvent>().Subscribe(this.OnSignedOutEvent);
        }
        #endregion

        #region Methods
        protected void OnSignedInEvent(object payload)
        {
            SignedIn = true;
        }

        protected void OnSignedOutEvent(object payload)
        {
            SignedIn = false;
        }
        #endregion
    }
}
