using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using MyLibrary.Constants;
using MyLibrary.ViewModels;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using cloudscribe.Pagination.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using MVCClient.Models;
using MyLibrary.Entities;
using JsonSerializer = System.Text.Json.JsonSerializer;

namespace MVCClient.Controllers
{
    [Authorize(Roles = MyIdentityServerConstants.Role_Admin)]
    public class UserAccountController : BaseController
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

        [HttpGet]
        public async Task<IActionResult> GetUserAccount(string id)
        {
            try
            {
                var accessToken = await HttpContext.GetTokenAsync("access_token");
                _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
                var content = await _client.GetStringAsync($"{MyAPIConstants.MyAPI_Url}UserAccounts/{id}"); 
                var userAccount = JsonConvert.DeserializeObject<UserAccountVwMdl>(content);

                return View("UserAccountDetails",userAccount);

            }
            catch(Exception ex)
            {
                return BadRequest(ex);
            }


        }

        /// <summary>
        /// Able or disable the admin role for a selected user account
        /// </summary>
        /// <param name="userId">user account id</param>
        /// <param name="isAdmin">new value of admin rights</param>
        /// <returns>true if succeeded, false if not</returns>
        [HttpPost]
        public async Task<bool> SetAdminStatusChange(string userId, bool isAdmin)
        {
            var content = await _client.PostAsync($"{MyIdentityServerConstants.IS_Url}Account/EditAdminRights?userid={userId}&isadmin={isAdmin}", null);
            if (content.IsSuccessStatusCode)
                return true;
            else return false ;
        }

        [HttpGet]
        public async Task<IActionResult> Delete(string id)
        {
            try
            {
                var accessToken = await HttpContext.GetTokenAsync("access_token");
                _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
                var content = await _client.GetStringAsync($"{MyAPIConstants.MyAPI_Url}UserAccounts/{id}");
                var userAccount = JsonConvert.DeserializeObject<UserAccountVwMdl>(content);

                return View(userAccount);

            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(string userId, string username)
        {
            var accessToken = await HttpContext.GetTokenAsync("access_token");
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

            var content = await _client.DeleteAsync($"{MyAPIConstants.MyAPI_Url}UserAccounts/Delete/{userId}");
            if (content.IsSuccessStatusCode)
            {
                AddSuccessMessage("Account deleted",$"{username} Account has been successfully deleted.");         
                return RedirectToAction("Index");
            }
            else
            {
                AddErrorMessage("Account not deleted",$"{username}Account has not been deleted due to some issues.");
                return RedirectToAction("Index");
            }
        }
    }
}
