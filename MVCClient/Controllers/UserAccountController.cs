using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using MyLibrary.Constants;
using MyLibrary.ViewModels;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using cloudscribe.Pagination.Models;
using Microsoft.AspNetCore.Authorization;

namespace MVCClient.Controllers
{
    public class UserAccountController : Controller
    {
        private readonly HttpClient _client;
        public UserAccountController()
        {
            _client = new HttpClient();
        }

        /// <summary>
        /// Get all user accounts
        /// </summary>
        /// <param name="sortOrder">sort order param</param>
        /// <param name="searchString">user search param</param>
        /// <param name="pageNumber"></param>
        /// <param name="pageSize">max items per page</param>
        /// <returns>paginated view with all (filtered) user accounts</returns>
        [HttpGet]
        [Authorize(Roles = MyIdentityServerConstants.Role_Admin)]
        public async Task<IActionResult> Index(string sortOrder, string searchString, int pageNumber = 1, int pageSize=10)
        {
            ViewData["UsernameSortParm"] = String.IsNullOrEmpty(sortOrder) ? "username_desc" : "";
            ViewData["EmailSortParm"] = sortOrder == "email" ? "email_desc" : "email";
            ViewData["IsProSortParm"] = sortOrder == "IsPro" ? "IsPro_desc" : "IsPro";
            ViewData["IsAdminSortParm"] = sortOrder == "IsAdmin" ? "IsAdmin_desc" : "IsAdmin";
            ViewData["CurrentFilter"] = searchString;

            var accessToken = await HttpContext.GetTokenAsync("access_token");

            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
            var content = await _client.GetStringAsync(MyAPIConstants.MyAPI_Url + "UserAccounts/GetAll");

            var unsortedModelsList = JsonConvert.DeserializeObject<List<UserAccountAdministrationVwMdl>>(content);
            if (!String.IsNullOrEmpty(searchString))
            {
                unsortedModelsList = unsortedModelsList.FindAll(x =>
                    x.Username.Contains(searchString));
            }
            IOrderedEnumerable<UserAccountAdministrationVwMdl> sortedModelsList;
            switch (sortOrder)
            {
                case "username_desc":
                    sortedModelsList = unsortedModelsList.OrderByDescending(s => s.Username);
                    break;
                case "email":
                    sortedModelsList = unsortedModelsList.OrderBy(s => s.Email);
                    break;
                case "email_desc":
                    sortedModelsList = unsortedModelsList.OrderByDescending(s => s.Email);
                    break;
                case "IsPro":
                    sortedModelsList = unsortedModelsList.OrderBy(s => s.IsProfessional);
                    break;
                case "IsPro_desc":
                    sortedModelsList = unsortedModelsList.OrderByDescending(s => s.IsProfessional);
                    break;
                case "IsAdmin":
                    sortedModelsList = unsortedModelsList.OrderBy(s => s.IsAdmin);
                    break;
                case "IsAdmin_desc":
                    sortedModelsList = unsortedModelsList.OrderByDescending(s => s.IsAdmin);
                    break;
                default:
                    sortedModelsList = unsortedModelsList.OrderBy(s => s.Username);
                    break;
            }

            int excludeRecords = (pageSize * pageNumber) - pageSize;
            var paginatedList = sortedModelsList.Skip(excludeRecords).Take(pageSize);
            var pageResult = new PagedResult<UserAccountAdministrationVwMdl>
            {
                Data = paginatedList.ToList(),
                TotalItems = sortedModelsList.Count(),
                PageNumber = pageNumber,
                PageSize = pageSize
            };
            return View(pageResult);
        }


    }
}
