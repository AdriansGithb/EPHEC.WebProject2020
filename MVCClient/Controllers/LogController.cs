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
using Microsoft.Extensions.Logging;
using MVCClient.Models;
using MyLibrary.Constants;
using MyLibrary.Entities;
using Newtonsoft.Json.Linq;

namespace MVCClient.Controllers
{
    public class LogController : Controller
    {
        private readonly ILogger<LogController> _logger;

        public LogController(ILogger<LogController> logger)
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

        [Authorize(Roles = MyIdentityServerConstants.Role_AdminOrManagerOrUser)]
        public IActionResult Logout()
        {
            return SignOut("Cookies", "oidc");
        }

        [AllowAnonymous]
        public IActionResult Register()
        {
            return Redirect(MyIdentityServerConstants.ISRegister_Url);
        }


    }
}
