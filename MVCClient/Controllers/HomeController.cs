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
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using MVCClient.Models;
using MyLibrary.Constants;
using Newtonsoft.Json.Linq;

namespace MVCClient.Controllers
{
    public class HomeController : BaseController
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }
        
        //public IActionResult Login()
        //{
        //    return Challenge(new AuthenticationProperties
        //        {
        //            RedirectUri = "/Home/Index"
        //        },
        //        "oidc");
        //}

        [AllowAnonymous]
        public IActionResult Index()
        {
            CheckIfToastrCookieAndShowIt();
            return View();
        }

        [Authorize(Roles = MyIdentityServerConstants.Role_User)]
        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        //[Authorize(Roles = MyIdentityServerConstants.Role_User)]
        //public IActionResult Logout()
        //{
        //    return SignOut("Cookies", "oidc");
        //}

        //You can access the tokens in the session using the standard ASP.NET Core extension methods
        //var accessToken = await HttpContext.GetTokenAsync("access_token");
        //For accessing the API using the access token, all you need to do is retrieve the token, and set it on your HttpClient:
        [Authorize(Roles = MyIdentityServerConstants.Role_Manager)]
        public async Task<IActionResult> CallApi()
        {
            var accessToken = await HttpContext.GetTokenAsync("access_token");

            var client = new HttpClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
            var content = await client.GetStringAsync(MyAPIConstants.MyAPI_IdentityCtrlr_Url);

            ViewBag.Json = JArray.Parse(content).ToString();
            return View("json");
        }

        [AllowAnonymous]
        public IActionResult AccessDenied()
        {
            return View();
        }

    }
}
