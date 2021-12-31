using Prism.Commands;
using Prism.Events;
using Prism.Mvvm;
using Stock.WpfClient.Common.Events;
using Stock.WpfClient.Common.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using Stock.WpfClient.Common.Models;

namespace Stock.WpfClient.Authentication.ViewModels
{
    public class AuthenticationViewModel : BindableBase, IAuthenticationViewModel
    {
        #region Fields
        private readonly IAuthenticationService _AuthenticationService;
        private readonly IEventAggregator _EventAggregator;
        #endregion

        #region Properties
        private string firstName = string.Empty;
        public string FirstName
        {
            get { return firstName; }
            set
            {
                SetProperty(ref this.firstName, value);
            }
        }

        private Visibility signedInVisibility = Visibility.Collapsed;
        public Visibility SignedInVisibility
        {
            get { return signedInVisibility; }
            set
            {
                SetProperty(ref this.signedInVisibility, value);
            }
        }

        private Visibility signedOutVisibility = Visibility.Visible;
        public Visibility SignedOutVisibility
        {
            get { return signedOutVisibility; }
            set
            {
                SetProperty(ref this.signedOutVisibility, value);
            }
        }

        public ICommand SignInCommand
        {
            get;
            set;
        }

        public ICommand SignOutCommand
        {
            get;
            set;
        }
        #endregion

        #region Constructor
        public AuthenticationViewModel(IAuthenticationService authenticationService,IEventAggregator eventAggregator)
        {
            _AuthenticationService = authenticationService;
            _EventAggregator = eventAggregator;
            Initialize();
        }
        #endregion

        #region Methods
        protected void Initialize()
        {
            //commands
            this.SignInCommand = new DelegateCommand(OnSignInExecuted);
            this.SignOutCommand = new DelegateCommand(OnSignOutExecuted);

            //delegates
            _OnSignInDelegate = new OnSignInDelegate(OnSignIn);
        }

        protected void OnSignInExecuted()
        {
            Task.Run(async () =>
                     {
                         var signedInResult = await _AuthenticationService.SignIn();
                         Application.Current.Dispatcher.Invoke(_OnSignInDelegate, signedInResult);
                     });
        }

        protected void OnSignOutExecuted()
        {
            Task.Run(() =>
                     {
                         _AuthenticationService.SignOut();
                     });
            this.SignedInVisibility = Visibility.Collapsed;
            this.SignedOutVisibility = Visibility.Visible;
            this.FirstName = string.Empty;
        }

        #endregion

        #region Delegates

        protected delegate void OnSignInDelegate(SignInResult signInResult);

        protected OnSignInDelegate _OnSignInDelegate;

        protected void OnSignIn(SignInResult signInResult)
        {
            if (!signInResult.HasError)
            {
                this.SignedInVisibility = Visibility.Visible;
                this.SignedOutVisibility = Visibility.Collapsed;
                this.FirstName = signInResult.User.FirstName ?? string.Empty;
            }
            else
            {
                //log error

            }
        }

        #endregion
    }
}
