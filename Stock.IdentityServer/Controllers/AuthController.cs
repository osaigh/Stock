using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Duende.IdentityModel;
using Duende.IdentityServer.Services;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Stock.IdentityServer.Models;
using Stock.IdentityServer.ViewModels;

namespace Stock.IdentityServer.Controllers
{
    public class AuthController : Controller
    {
        #region Fields
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IIdentityServerInteractionService _identityServerInteractionService;
        #endregion

        #region Constructor
        public AuthController(
            SignInManager<ApplicationUser> signManager, 
            UserManager<ApplicationUser> userManager,
            IIdentityServerInteractionService identityServerInteractionService
            )
        {
            _signInManager = signManager;
            _userManager = userManager;
            _identityServerInteractionService = identityServerInteractionService;
        }
        #endregion

        #region Methods
        /// <summary>
        /// Login
        /// </summary>
        /// <param name="returnUrl"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> Login(string returnUrl)
        {
            var loginViewModel = new LoginViewModel()
                                 {
                                     ReturnUrl = returnUrl,
                                     ExternalProviders = await GetExternalProvidersAsync()
                                 };
            return View(loginViewModel);
        }

        /// <summary>
        /// Login
        /// </summary>
        /// <param name="loginViewModel"></param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel loginViewModel)
        {
            if (ModelState.IsValid)
            {
                var result = await _signInManager.PasswordSignInAsync(loginViewModel.Username, loginViewModel.Password, false, false);

                if (result.Succeeded)
                {
                    return RedirectToReturnUrl(loginViewModel.ReturnUrl);
                }

                ModelState.AddModelError(string.Empty, "Invalid username or password");
            }

            loginViewModel.ExternalProviders = await GetExternalProvidersAsync();
            return View(loginViewModel);
        }

        [HttpGet]
        public IActionResult Register(string returnUrl)
        {
            var registerViewModel = new RegisterViewModel()
                                    {
                                        ReturnUrl = returnUrl
                                    };

            return View(registerViewModel);
        }

        /// <summary>
        /// Register a new user
        /// </summary>
        /// <param name="registerViewModel"></param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterViewModel registerViewModel)
        {
            if (!ModelState.IsValid)
            {
                return View(registerViewModel);
            }
            ApplicationUser user = new ApplicationUser()
                                   {
                                       FirstName = registerViewModel.FirstName,
                                       LastName = registerViewModel.LastName,
                                       UserName = registerViewModel.Username,
                                       Email = registerViewModel.Email
                                   };

            var result = await _userManager.CreateAsync(user,registerViewModel.Password);

            if (result.Succeeded)
            { 
                await _userManager.AddClaimsAsync(user, GetNameClaims(user));
                await _signInManager.SignInAsync(user, false);
                return RedirectToReturnUrl(registerViewModel.ReturnUrl);
            }

            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }

            return View(registerViewModel);
        }

        /// <summary>
        /// Log out
        /// </summary>
        /// <param name="logoutId"></param>
        /// <returns></returns>
        public async Task<IActionResult> Logout(string logoutId)
        {
            await _signInManager.SignOutAsync();
            var logoutRequest = await _identityServerInteractionService.GetLogoutContextAsync(logoutId, HttpContext.RequestAborted);
            

            if (string.IsNullOrEmpty(logoutRequest?.PostLogoutRedirectUri))
            {
                return RedirectToAction("Login", "Auth");
            }

            return Redirect(logoutRequest.PostLogoutRedirectUri);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult ExternalLogin(string provider ,string returnUrl)
        {
            var redirectUri = Url.Action(nameof(ExternalLoginCallback), "Auth", new {returnUrl});
            var externalAuthProperties = _signInManager.ConfigureExternalAuthenticationProperties(provider,redirectUri);
            return Challenge(externalAuthProperties, provider);
        }

        public async Task<IActionResult> ExternalLoginCallback(string returnUrl)
        {
            var info = await _signInManager.GetExternalLoginInfoAsync();

            //Nothing happened
            if (info == null)
            {
                return RedirectToAction("Login", new { returnUrl });
            }

            var result = await _signInManager.ExternalLoginSignInAsync(info.LoginProvider,info.ProviderKey,false);

            ///To be continued
            if (result.Succeeded)
            {
                return RedirectToReturnUrl(returnUrl);
            }

            return RedirectToAction("Login", new { returnUrl });
        }

        /// <summary>
        /// The given_name and family_name claims that are issued with the profile scope
        /// </summary>
        public static IEnumerable<Claim> GetNameClaims(ApplicationUser user)
        {
            var claims = new List<Claim>();

            if (!string.IsNullOrEmpty(user.FirstName))
            {
                claims.Add(new Claim(JwtClaimTypes.GivenName, user.FirstName));
            }

            if (!string.IsNullOrEmpty(user.LastName))
            {
                claims.Add(new Claim(JwtClaimTypes.FamilyName, user.LastName));
            }

            return claims;
        }

        private async Task<IEnumerable<AuthenticationScheme>> GetExternalProvidersAsync()
        {
            return await _signInManager.GetExternalAuthenticationSchemesAsync() ?? new List<AuthenticationScheme>();
        }

        /// <summary>
        /// Redirects to the return url when it is safe to do so: either a local url (for example the
        /// authorize callback of a sign in that is in progress) or a url of one of the registered clients.
        /// Anything else goes to the home page, so the server cannot be used as an open redirect.
        /// </summary>
        private IActionResult RedirectToReturnUrl(string returnUrl)
        {
            if (!string.IsNullOrEmpty(returnUrl))
            {
                if (Url.IsLocalUrl(returnUrl) || _identityServerInteractionService.IsValidReturnUrl(returnUrl))
                {
                    return Redirect(returnUrl);
                }

                if (Uri.TryCreate(returnUrl, UriKind.Absolute, out Uri uri)
                    && Configuration.GetClients()
                                    .SelectMany(client => client.AllowedCorsOrigins)
                                    .Contains(uri.GetLeftPart(UriPartial.Authority), StringComparer.OrdinalIgnoreCase))
                {
                    return Redirect(returnUrl);
                }
            }

            return RedirectToAction("Index", "Home");
        }
        #endregion
    }
}
