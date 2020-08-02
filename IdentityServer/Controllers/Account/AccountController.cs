// Copyright (c) Brock Allen & Dominick Baier. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.


using IdentityModel;
using IdentityServer4.Events;
using IdentityServer4.Extensions;
using IdentityServer4.Models;
using IdentityServer4.Services;
using IdentityServer4.Stores;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using System.Threading.Tasks;
using IdentityServer.Controllers.Base;
using IdentityServer.Models.Account;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.EntityFrameworkCore;
using MyLibrary.Constants;
using MyLibrary.Entities;
using MyLibrary.ViewModels;

namespace IdentityServerHost.Quickstart.UI
{
    [SecurityHeaders]
    [AllowAnonymous]
    public class AccountController : BaseController
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly IIdentityServerInteractionService _interaction;
        private readonly IClientStore _clientStore;
        private readonly IAuthenticationSchemeProvider _schemeProvider;
        private readonly IEventService _events;

        public AccountController(
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            IIdentityServerInteractionService interaction,
            IClientStore clientStore,
            IAuthenticationSchemeProvider schemeProvider,
            IEventService events
        )
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _interaction = interaction;
            _clientStore = clientStore;
            _schemeProvider = schemeProvider;
            _events = events;
        }
        /// <summary>
        /// Entry point into the registration workflow
        /// </summary>
        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        /// <summary>
        /// Handle postback from new user registration
        /// </summary>
        /// <param name="regFormData">Registration model with user details</param>
        /// <returns>Redirect to MVCClient HomePage, with the new user logged</returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterVwMdl regFormData)
        {
            if (ModelState.IsValid)
            {
                ApplicationUser newUser = new ApplicationUser
                {
                    UserName = regFormData.UserDetails.Email,
                    Email = regFormData.UserDetails.Email,
                    EmailConfirmed = true,
                    LastName = regFormData.UserDetails.LastName,
                    FirstName = regFormData.UserDetails.FirstName,
                    BirthDate = regFormData.UserDetails.BirthDate,
                    GenderType_Id = regFormData.UserDetails.GenderType_Id,
                    IsAdmin = false,
                    IsProfessional = regFormData.UserDetails.IsProfessional,
                };

                if (regFormData.UserDetails.PhoneNumber != null)
                {
                    newUser.PhoneNumber = regFormData.UserDetails.PhoneNumber;
                    newUser.PhoneNumberConfirmed = true;
                }

                var resultReg = await _userManager.CreateAsync(newUser, regFormData.Password);
                if (!resultReg.Succeeded)
                {
                    foreach (IdentityError error in resultReg.Errors)
                    { 
                        ModelState.TryAddModelError(error.Code,error.Description);
                    }
                    return View(regFormData);
                }

                await _userManager.AddToRoleAsync(newUser, MyIdentityServerConstants.Role_User);

                if (newUser.IsProfessional)
                {
                    await _userManager.AddToRoleAsync(newUser, MyIdentityServerConstants.Role_Manager);
                }

                var resultLog = await _signInManager.PasswordSignInAsync(newUser.UserName, regFormData.Password, false ,lockoutOnFailure: true);
                if (resultLog.Succeeded)
                {
                    var user = await _userManager.FindByNameAsync(newUser.UserName);
                    await _events.RaiseAsync(new UserLoginSuccessEvent(user.UserName, user.Id, user.UserName));
                    AddSuccessMessage("Registration succeed",$"Congratulations {user.FirstName}, you have been successfully logged and registered. Welcome in our members !");
                    return Redirect(MyMVCConstants.MyMVC_Login_Url);
                }

                await _events.RaiseAsync(new UserLoginFailureEvent(newUser.UserName, "invalid credentials"));
                AddErrorMessage("Login failure", "Registration completed successfully but your login credentials failed. Please try again to log in.");
                return Redirect(MyMVCConstants.MyMVC_Login_Url);
            }
            else
            {
                return View(regFormData);
            }

        }

        /// <summary>
        /// Entry point for User Account Details edition
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> EditUserDetails()
        {
            var user = await _userManager.FindByNameAsync(User.Identity.Name);
            UserAccountEditionVwMdl userData = new UserAccountEditionVwMdl()
            {
                UserDetails = user
            };
            return View(userData);
        }

        /// <summary>
        /// Handle User Account Details edition
        /// </summary>
        /// <param name="editedUserData"></param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditUserDetails(UserAccountEditionVwMdl editedUserData)
        {
            if (ModelState.IsValid)
            {
                var modifiedUser = await _userManager.FindByIdAsync(editedUserData.UserDetails.Id);
                var resultPass =
                    await _userManager.CheckPasswordAsync(modifiedUser, editedUserData.CurrentPassword);
                if (resultPass)
                {
                    modifiedUser.UserName = editedUserData.UserDetails.Email;
                    modifiedUser.Email = editedUserData.UserDetails.Email;
                    modifiedUser.EmailConfirmed = true;
                    modifiedUser.LastName = editedUserData.UserDetails.LastName;
                    modifiedUser.FirstName = editedUserData.UserDetails.FirstName;
                    modifiedUser.BirthDate = editedUserData.UserDetails.BirthDate;
                    modifiedUser.GenderType_Id = editedUserData.UserDetails.GenderType_Id;

                    if (editedUserData.UserDetails.PhoneNumber != null)
                    {
                        modifiedUser.PhoneNumber = editedUserData.UserDetails.PhoneNumber;
                        modifiedUser.PhoneNumberConfirmed = true;
                    }
                    else
                    {
                        modifiedUser.PhoneNumber = null;
                        modifiedUser.PhoneNumberConfirmed = false;
                    }

                    bool profActivated = false;
                    if (editedUserData.UserDetails.IsProfessional != modifiedUser.IsProfessional)
                    {
                        if (editedUserData.UserDetails.IsProfessional)
                        {
                            modifiedUser.IsProfessional = true;
                            profActivated = true;
                        }
                        else
                        {
                            ModelState.AddModelError("", "Disable a professional account is prohibited. " +
                                                         "If you want to not manage any establishment anymore, please delete your account and create a new non professional one.");
                            ModelState.SetModelValue("UserDetails.IsProfessional", new ValueProviderResult("true"));
                            return View(editedUserData);
                        }
                    }
                            
                    var resultEdit = await _userManager.UpdateAsync(modifiedUser);
                    if (!resultEdit.Succeeded)
                    {
                        foreach (IdentityError error in resultEdit.Errors)
                        {
                            ModelState.TryAddModelError(error.Code, error.Description);
                        }

                        return View(editedUserData);
                    }

                    if (profActivated)
                    {
                        await _userManager.AddToRoleAsync(modifiedUser,
                            MyIdentityServerConstants.Role_Manager);
                    }

                    if (!editedUserData.NewPassword.IsNullOrEmpty())
                    {
                        var resultPassEdit = await _userManager.ChangePasswordAsync(modifiedUser,
                            editedUserData.CurrentPassword, editedUserData.NewPassword);
                        if (!resultPassEdit.Succeeded)
                        {
                            ModelState.AddModelError("",
                                "There was a problem with your password modification, please fill in the current/new/confirmation password fields and try again.");
                            return View(editedUserData);
                        }
                    }

                    await _signInManager.RefreshSignInAsync(modifiedUser);

                    AddSuccessMessage("Account modifications saved", "Your user account details have been successfully saved.");
                    return Redirect(MyMVCConstants.MyMVC_Login_Url);

                }
                else
                {
                    ModelState.AddModelError("","Invalid current password, please try again");
                    return View(editedUserData);
                }
            }
            else
            {
                return View(editedUserData);
            }
        }

        /// <summary>
        /// Entry point into the login workflow
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> Login(string returnUrl)
        {
            // build a model so we know what to show on the login page
            var vm = await BuildLoginViewModelAsync(returnUrl);

            if (vm.IsExternalLoginOnly)
            {
                // we only have one option for logging in and it's an external provider
                return RedirectToAction("Challenge", "External", new { scheme = vm.ExternalLoginScheme, returnUrl });
            }

            return View(vm);
        }

        /// <summary>
        /// Handle postback from username/password login
        /// </summary>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginInputModel model, string button)
        {
            // check if we are in the context of an authorization request
            var context = await _interaction.GetAuthorizationContextAsync(model.ReturnUrl);

            // the user clicked the "cancel" button
            if (button != "login")
            {
                if (context != null)
                {
                    // if the user cancels, send a result back into IdentityServer as if they 
                    // denied the consent (even if this client does not require consent).
                    // this will send back an access denied OIDC error response to the client.
                    await _interaction.DenyAuthorizationAsync(context, AuthorizationError.AccessDenied);

                    // we can trust model.ReturnUrl since GetAuthorizationContextAsync returned non-null
                    if (context.IsNativeClient())
                    {
                        // The client is native, so this change in how to
                        // return the response is for better UX for the end user.
                        return this.LoadingPage("Redirect", model.ReturnUrl);
                    }

                    return Redirect(MyMVCConstants.MyMVC_Url);
                }
                else
                {
                    // since we don't have a valid context, then we just go back to the home page
                    return Redirect("~/");
                }
            }

            if (ModelState.IsValid)
            {
                var result = await _signInManager.PasswordSignInAsync(model.Username, model.Password, model.RememberLogin, lockoutOnFailure: true);
                if (result.Succeeded)
                {
                    var user = await _userManager.FindByNameAsync(model.Username);
                    await _events.RaiseAsync(new UserLoginSuccessEvent(user.UserName, user.Id, user.UserName, clientId: context?.Client.ClientId));

                    if (context != null)
                    {
                        if (context.IsNativeClient())
                        {
                            // The client is native, so this change in how to
                            // return the response is for better UX for the end user.
                            return this.LoadingPage("Redirect", model.ReturnUrl);
                        }
                        //notification cookie
                        AddSuccessMessage("Login succeed",$"Welcome {user.FirstName} !");
                        // we can trust model.ReturnUrl since GetAuthorizationContextAsync returned non-null
                        return Redirect(model.ReturnUrl);
                    }

                    // request for a local page
                    if (Url.IsLocalUrl(model.ReturnUrl))
                    {
                        return Redirect(model.ReturnUrl);
                    }
                    else if (string.IsNullOrEmpty(model.ReturnUrl))
                    {
                        return Redirect("~/");
                    }
                    else
                    {
                        // user might have clicked on a malicious link - should be logged
                        throw new Exception("invalid return URL");
                    }
                }

                await _events.RaiseAsync(new UserLoginFailureEvent(model.Username, "invalid credentials", clientId:context?.Client.ClientId));
                ModelState.AddModelError(string.Empty, AccountOptions.InvalidCredentialsErrorMessage);
            }

            // something went wrong, show form with error
            var vm = await BuildLoginViewModelAsync(model);
            return View(vm);
        }

        
        /// <summary>
        /// Show logout page
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> Logout(string logoutId)
        {
            // build a model so the logout page knows what to display
            var vm = await BuildLogoutViewModelAsync(logoutId);

            if (vm.ShowLogoutPrompt == false)
            {
                // if the request for logout was properly authenticated from IdentityServer, then
                // we don't need to show the prompt and can just log the user out directly.
                return await Logout(vm);
            }

            return View(vm);
        }

        /// <summary>
        /// Handle logout page postback
        /// </summary>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Logout(LogoutInputModel model)
        {
            // build a model so the logged out page knows what to display
            var vm = await BuildLoggedOutViewModelAsync(model.LogoutId);

            if (User?.Identity.IsAuthenticated == true)
            {
                // delete local authentication cookie
                await _signInManager.SignOutAsync();

                // raise the logout event
                await _events.RaiseAsync(new UserLogoutSuccessEvent(User.GetSubjectId(), User.GetDisplayName()));
            }

            // check if we need to trigger sign-out at an upstream identity provider
            if (vm.TriggerExternalSignout)
            {
                // build a return URL so the upstream provider will redirect back
                // to us after the user has logged out. this allows us to then
                // complete our single sign-out processing.
                string url = Url.Action("Logout", new { logoutId = vm.LogoutId });

                // this triggers a redirect to the external provider for sign-out
                return SignOut(new AuthenticationProperties { RedirectUri = url }, vm.ExternalAuthenticationScheme);
            }

            return View("LoggedOut", vm);
        }

        [HttpGet]
        public IActionResult AccessDenied()
        {
            return View();
        }


        /*****************************************/
        /* helper APIs for the AccountController */
        /*****************************************/
        private async Task<LoginViewModel> BuildLoginViewModelAsync(string returnUrl)
        {
            var context = await _interaction.GetAuthorizationContextAsync(returnUrl);
            if (context?.IdP != null && await _schemeProvider.GetSchemeAsync(context.IdP) != null)
            {
                var local = context.IdP == IdentityServer4.IdentityServerConstants.LocalIdentityProvider;

                // this is meant to short circuit the UI and only trigger the one external IdP
                var vm = new LoginViewModel
                {
                    EnableLocalLogin = local,
                    ReturnUrl = returnUrl,
                    Username = context?.LoginHint,
                };

                if (!local)
                {
                    vm.ExternalProviders = new[] { new ExternalProvider { AuthenticationScheme = context.IdP } };
                }

                return vm;
            }

            var schemes = await _schemeProvider.GetAllSchemesAsync();

            var providers = schemes
                .Where(x => x.DisplayName != null)
                .Select(x => new ExternalProvider
                {
                    DisplayName = x.DisplayName ?? x.Name,
                    AuthenticationScheme = x.Name
                }).ToList();

            var allowLocal = true;
            if (context?.Client.ClientId != null)
            {
                var client = await _clientStore.FindEnabledClientByIdAsync(context.Client.ClientId);
                if (client != null)
                {
                    allowLocal = client.EnableLocalLogin;

                    if (client.IdentityProviderRestrictions != null && client.IdentityProviderRestrictions.Any())
                    {
                        providers = providers.Where(provider => client.IdentityProviderRestrictions.Contains(provider.AuthenticationScheme)).ToList();
                    }
                }
            }

            return new LoginViewModel
            {
                AllowRememberLogin = AccountOptions.AllowRememberLogin,
                EnableLocalLogin = allowLocal && AccountOptions.AllowLocalLogin,
                ReturnUrl = returnUrl,
                Username = context?.LoginHint,
                ExternalProviders = providers.ToArray()
            };
        }

        private async Task<LoginViewModel> BuildLoginViewModelAsync(LoginInputModel model)
        {
            var vm = await BuildLoginViewModelAsync(model.ReturnUrl);
            vm.Username = model.Username;
            vm.RememberLogin = model.RememberLogin;
            return vm;
        }

        private async Task<LogoutViewModel> BuildLogoutViewModelAsync(string logoutId)
        {
            var vm = new LogoutViewModel { LogoutId = logoutId, ShowLogoutPrompt = AccountOptions.ShowLogoutPrompt };

            if (User?.Identity.IsAuthenticated != true)
            {
                // if the user is not authenticated, then just show logged out page
                vm.ShowLogoutPrompt = false;
                return vm;
            }

            var context = await _interaction.GetLogoutContextAsync(logoutId);
            if (context?.ShowSignoutPrompt == false)
            {
                // it's safe to automatically sign-out
                vm.ShowLogoutPrompt = false;
                return vm;
            }

            // show the logout prompt. this prevents attacks where the user
            // is automatically signed out by another malicious web page.
            return vm;
        }

        private async Task<LoggedOutViewModel> BuildLoggedOutViewModelAsync(string logoutId)
        {
            // get context information (client name, post logout redirect URI and iframe for federated signout)
            var logout = await _interaction.GetLogoutContextAsync(logoutId);

            var vm = new LoggedOutViewModel
            {
                AutomaticRedirectAfterSignOut = AccountOptions.AutomaticRedirectAfterSignOut,
                PostLogoutRedirectUri = logout?.PostLogoutRedirectUri,
                ClientName = string.IsNullOrEmpty(logout?.ClientName) ? logout?.ClientId : logout?.ClientName,
                SignOutIframeUrl = logout?.SignOutIFrameUrl,
                LogoutId = logoutId
            };

            if (User?.Identity.IsAuthenticated == true)
            {
                var idp = User.FindFirst(JwtClaimTypes.IdentityProvider)?.Value;
                if (idp != null && idp != IdentityServer4.IdentityServerConstants.LocalIdentityProvider)
                {
                    var providerSupportsSignout = await HttpContext.GetSchemeSupportsSignOutAsync(idp);
                    if (providerSupportsSignout)
                    {
                        if (vm.LogoutId == null)
                        {
                            // if there's no current logout context, we need to create one
                            // this captures necessary info from the current logged in user
                            // before we signout and redirect away to the external IdP for signout
                            vm.LogoutId = await _interaction.CreateLogoutContextAsync();
                        }

                        vm.ExternalAuthenticationScheme = idp;
                    }
                }
            }

            return vm;
        }
    }
}