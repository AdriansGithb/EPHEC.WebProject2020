using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.Extensions.Logging;
using MVCClient.Models;
using MyLibrary.Constants;
using MyLibrary.Entities;
using Newtonsoft.Json.Linq;

namespace MVCClient.Controllers
{
    public class AccountController : BaseController
    {
        private readonly ILogger<AccountController> _logger;

        public AccountController(ILogger<AccountController> logger)
        {
            _logger = logger;
        }

        [AllowAnonymous]
        public IActionResult Login()
        {
            return Challenge(new AuthenticationProperties
                {
                    RedirectUri = "/Home/Index"
                },
                "oidc");
        }


        [Authorize(Roles = MyIdentityServerConstants.Role_User)]
        public IActionResult Logout()
        {
            return SignOut("Cookies", "oidc");
        }

        [AllowAnonymous]
        public IActionResult Register()
        {
            return Redirect(MyIdentityServerConstants.ISRegister_Url);
        }

        [Authorize]
        public IActionResult EditUserDetails()
        {
            return Redirect(MyIdentityServerConstants.ISUserEditing_Url);
        }

        [HttpGet]
        [Authorize(Roles = MyIdentityServerConstants.Role_User)]
        public IActionResult DeleteSelfAccount()
        {
            return View();
        }

        [HttpPost]
        [Authorize(Roles = MyIdentityServerConstants.Role_User)]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteSelfAccount(string userid, string username)
        {
            try
            {
                string checkId = User.Claims.First(x => x.Type.Contains("sub")).Value;
                string checkName = User.Identity.Name;
                if (checkName.Equals(username) && checkId.Equals(userid))
                {
                    HttpClient _client = new HttpClient();
                    var accessToken = await HttpContext.GetTokenAsync("access_token");
                    _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

                    var content = await _client.DeleteAsync($"{MyAPIConstants.MyAPI_Url}UserAccounts/Delete/{userid}");
                    if (content.IsSuccessStatusCode)
                    {
                        AddSuccessMessage("Account deleted", $"{username}, your account has been successfully deleted. Thanks for your participation, do not hesitate to come back whenever you want. Goodbye.");
                        return RedirectToAction("Logout");
                    }
                    else
                    {
                        AddErrorMessage("Account not deleted",
                            $"{username}Account has not been deleted due to some issues.");
                        return View();
                    }
                }
                else
                {
                    throw new Exception("It seems there is an issue between the view user claims and the controller user claims.");
                }
            }
            catch (Exception ex)
            {
                AddErrorMessage("Account not deleted", $"{username}, your account has not been deleted due to unknown error ({ex.Message})");
                return View();
            }
        }

    }
}
