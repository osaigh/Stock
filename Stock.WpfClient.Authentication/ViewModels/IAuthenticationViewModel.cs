using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace Stock.WpfClient.Authentication.ViewModels
{
    public interface IAuthenticationViewModel
    {
        string FirstName { get;}
        Visibility SignedInVisibility { get; }

        Visibility SignedOutVisibility { get; }

        ICommand SignInCommand { get; }

        ICommand SignOutCommand { get; }
    }
}
