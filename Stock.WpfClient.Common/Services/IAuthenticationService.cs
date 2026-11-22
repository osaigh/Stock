using Stock.WpfClient.Common.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Stock.WpfClient.Common.Services
{
    public interface IAuthenticationService
    {
        Task<SignInResult> SignIn();
        Task SignOut();

        User GetUserIdentity();
    }
}
