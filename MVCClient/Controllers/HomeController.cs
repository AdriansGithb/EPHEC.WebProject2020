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
using MyLibrary.Entities;
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

        [HttpGet]
        [AllowAnonymous]
        public async Task<string> GetGeocodedAddress(EstablishmentsAddresses Address)
        {
            try
            {
                string url = CreateMapApiQueryUrl(Address);
                
                var httpResponse = await _client.GetAsync(url);
                if (!httpResponse.IsSuccessStatusCode)
                {
                    return new string(httpResponse.ReasonPhrase);
                }

                var content = await httpResponse.Content.ReadAsStringAsync();

                return content;

            }
            catch (Exception ex)
            {
                AddErrorMessage("error",ex.Message);
                return "";
            }
        }

        private string CreateMapApiQueryUrl(EstablishmentsAddresses address)
        {
            try
            {
                //query structure : base + {address : houseNumber (box) street zipcode city country } + end + token + {options : &autocomplete=false&types=address&limit=1}
                string baseUrl = "https://api.mapbox.com/geocoding/v5/mapbox.places";
                string endUrl = ".json?access_token=";
                string options = "&autocomplete=false&types=address&limit=1";

                string fullAddress = $"{address.HouseNumber} {address.BoxNumber} {address.Street} {address.ZipCode} {address.City} {address.Country}";

                string fullUrlString = $"{baseUrl}/{address}{endUrl}{MyMVCConstants.MyMVC_MapBox_Token}{options}";
                //string apiQueryUrl = HttpUtility.UrlEncode(fullUrlString);
                return fullUrlString;

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
