using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Text;
using System.Text.Encodings.Web;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.WebEncoders.Testing;
using MVCClient.Models;
using MyLibrary.Constants;
using MyLibrary.DTOs;
using MyLibrary.Entities;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace MVCClient.Controllers
{
    public class HomeController : BaseController
    {
        private readonly ILogger<HomeController> _logger;
        private readonly HttpClient _client;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
            _client = new HttpClient();
        }

        [AllowAnonymous]
        public IActionResult Index()
        {
            if((!User.Identity.IsAuthenticated) &&(Request.Cookies.ContainsKey(".AspNetCore.Identity.Application")))
            {
                return Challenge(new AuthenticationProperties
                    {
                        RedirectUri = "/Home/Index"
                    },
                    "oidc");
            }
            CheckIfToastrCookieAndShowIt();
            return View();
        }
        
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        [AllowAnonymous]
        public IActionResult AccessDenied()
        {
            return View();
        }

        [AllowAnonymous]
        public async Task<JsonResult> GetAddresses()
        {
            try
            {
                HttpResponseMessage httpResponse = new HttpResponseMessage();
                if (User.IsInRole(MyIdentityServerConstants.Role_User))
                {
                    var accessToken = await HttpContext.GetTokenAsync("access_token");
                    _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

                    httpResponse = await _client.GetAsync($"{MyAPIConstants.MyAPI_EstabAddressesCtrl_Url}GetAllOpen");
                }
                else
                {
                    httpResponse = await _client.GetAsync($"{MyAPIConstants.MyAPI_EstabAddressesCtrl_Url}GetAll");
                }
                if (!httpResponse.IsSuccessStatusCode)
                {
                    throw new Exception(httpResponse.ReasonPhrase);
                }
                
                return Json(await httpResponse.Content.ReadAsStringAsync());
            }
            catch (Exception ex)
            {
                return Json(ex);
            }
        }

    }
}
