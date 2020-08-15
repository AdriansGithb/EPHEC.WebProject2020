using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using cloudscribe.Pagination.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MyLibrary.Constants;
using MyLibrary.DTOs;
using MyLibrary.ViewModels;
using Newtonsoft.Json;

namespace MVCClient.Controllers
{
    public class NewsController : BaseController
    {
        private readonly HttpClient _client;
        public NewsController()
        {
            _client = new HttpClient();
        }
        
        [HttpGet]
        [Authorize(Roles = MyIdentityServerConstants.Role_Admin)]
        public async Task<IActionResult> Index(string sortOrder, string searchString, int pageNumber = 1, int pageSize = 5)
        {
            try
            {
                ViewData["CreatedSortPrm"] = String.IsNullOrEmpty(sortOrder) ? "created_asc" : "";
                ViewData["EstabIdSortPrm"] = sortOrder == "estabId" ? "estabId_desc" : "estabId";
                ViewData["EstabNameSortPrm"] = sortOrder == "estabName" ? "estabName_desc" : "estabName";
                ViewData["TitleSortPrm"] = sortOrder == "title" ? "title_desc" : "title";
                ViewData["LastUpdtSortPrm"] = sortOrder == "lastUpdt" ? "lastUpdt_desc" : "lastUpdt";
                ViewData["CurrentFilter"] = searchString;

                var accessToken = await HttpContext.GetTokenAsync("access_token");
                _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);


                var httpResponse = await _client.GetAsync($"{MyAPIConstants.MyAPI_EstabNewsCtrl_Url}GetAll");
                if (!httpResponse.IsSuccessStatusCode)
                {
                    throw new Exception(httpResponse.ReasonPhrase);
                }

                var content = await httpResponse.Content.ReadAsStringAsync();
                var unsortedModelsList = JsonConvert.DeserializeObject<List<NewsDTO>>(content);
                if (!String.IsNullOrEmpty(searchString))
                {
                    unsortedModelsList = unsortedModelsList.FindAll(x =>
                        x.Title.Contains(searchString));
                }
                IOrderedEnumerable<NewsDTO> sortedModelsList;
                switch (sortOrder)
                {
                    case "created_asc":
                        sortedModelsList = unsortedModelsList.OrderBy(s => s.CreatedDate);
                        break;
                    case "estabId":
                        sortedModelsList = unsortedModelsList.OrderBy(s => s.EstablishmentId);
                        break;
                    case "estabId_desc":
                        sortedModelsList = unsortedModelsList.OrderByDescending(s => s.EstablishmentId);
                        break;
                    case "estabName":
                        sortedModelsList = unsortedModelsList.OrderBy(s => s.EstablishmentName);
                        break;
                    case "estabName_desc":
                        sortedModelsList = unsortedModelsList.OrderByDescending(s => s.EstablishmentName);
                        break;
                    case "title":
                        sortedModelsList = unsortedModelsList.OrderBy(s => s.Title);
                        break;
                    case "title_desc":
                        sortedModelsList = unsortedModelsList.OrderByDescending(s => s.Title);
                        break;
                    case "lastUpdt":
                        sortedModelsList = unsortedModelsList.OrderBy(s => s.UpdatedDate);
                        break;
                    case "lastUpdt_desc":
                        sortedModelsList = unsortedModelsList.OrderByDescending(s => s.UpdatedDate);
                        break;
                    default:
                        sortedModelsList = unsortedModelsList.OrderByDescending(s => s.CreatedDate);
                        break;
                }

                int excludeRecords = (pageSize * pageNumber) - pageSize;
                var paginatedList = sortedModelsList.Skip(excludeRecords).Take(pageSize);
                var pageResult = new PagedResult<NewsDTO>
                {
                    Data = paginatedList.ToList(),
                    TotalItems = sortedModelsList.Count(),
                    PageNumber = pageNumber,
                    PageSize = pageSize
                };
                return View(pageResult);
            }
            catch (Exception ex)
            {
                AddErrorMessage("Error", ex.Message);
                return RedirectToAction("Index","Home");
            }
        }

        [HttpGet]
        [Authorize(Roles = MyIdentityServerConstants.Role_Admin)]
        public IActionResult Create(int estabId, string estabName)
        {
            try
            {
                EstablishmentNewsVwMdl model = new EstablishmentNewsVwMdl
                {
                    EstablishmentId = estabId,
                    EstablishmentName = estabName
                };
                return View(model);
            }
            catch (Exception ex)
            {
                AddErrorMessage("Error", ex.Message);
                return RedirectToAction("Index", "Establishments");
            }
        }

        [HttpPost]
        [Authorize(Roles = MyIdentityServerConstants.Role_Admin)]
        public async Task<IActionResult> Create(EstablishmentNewsVwMdl model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var accessToken = await HttpContext.GetTokenAsync("access_token");
                    _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

                    var postContent = JsonConvert.SerializeObject(model);
                    var httpContent = new StringContent(postContent, Encoding.Default, "application/json");
                    var httpResponse = await _client.PostAsync($"{MyAPIConstants.MyAPI_EstabNewsCtrl_Url}Create", httpContent);

                    if (!httpResponse.IsSuccessStatusCode)
                    {
                        AddErrorMessage("Not created", "Your news has not been saved due to some issues. Error : "+httpResponse.ReasonPhrase);
                        return View(model);
                    }

                    AddSuccessMessage("News created", "Your news has been created successfully");
                    return RedirectToAction("Index");

                }
                else return View(model);
            }
            catch (Exception ex)
            {
                AddErrorMessage("Error", ex.Message);
                return RedirectToAction("Index","Establishments");
            }
        }

        [HttpGet]
        [Authorize(Roles = MyIdentityServerConstants.Role_Admin)]
        public async Task<IActionResult> Edit(string newsId)
        {
            try
            {
                var accessToken = await HttpContext.GetTokenAsync("access_token");
                _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

                var httpResponse = await _client.GetAsync($"{MyAPIConstants.MyAPI_EstabNewsCtrl_Url}Get/{newsId}");
                if (!httpResponse.IsSuccessStatusCode)
                {
                    AddErrorMessage("Error", httpResponse.ReasonPhrase);
                    return RedirectToAction("Index");
                }

                var content = await httpResponse.Content.ReadAsStringAsync();
                EstablishmentNewsVwMdl news = JsonConvert.DeserializeObject<EstablishmentNewsVwMdl>(content);

                return View(news);

            }
            catch (Exception ex)
            {
                AddErrorMessage("Error", ex.Message);
                return RedirectToAction("Index", "Establishments");
            }
        }

        [HttpPost]
        [Authorize(Roles = MyIdentityServerConstants.Role_Admin)]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(EstablishmentNewsVwMdl model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var accessToken = await HttpContext.GetTokenAsync("access_token");
                    _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

                    var postContent = JsonConvert.SerializeObject(model);
                    var httpContent = new StringContent(postContent, Encoding.Default, "application/json");
                    var httpResponse = await _client.PutAsync($"{MyAPIConstants.MyAPI_EstabNewsCtrl_Url}Edit", httpContent);

                    if (!httpResponse.IsSuccessStatusCode)
                    {
                        AddErrorMessage("Not edited", "Your news has not been saved due to some issues. Error : " + httpResponse.ReasonPhrase);
                        return View(model);
                    }

                    AddSuccessMessage("News edited", "Your news has been edited successfully");
                    return RedirectToAction("Index");

                }
                else return View(model);
            }
            catch (Exception ex)
            {
                AddErrorMessage("Error", ex.Message);
                return RedirectToAction("Index", "Establishments");
            }
        }

        [HttpGet]
        public async Task<bool> Delete(string newsId)
        {
            try
            {
                    var accessToken = await HttpContext.GetTokenAsync("access_token");
                    _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

                    var httpResponse = await _client.DeleteAsync($"{MyAPIConstants.MyAPI_EstabNewsCtrl_Url}Delete/{newsId}");

                    if (!httpResponse.IsSuccessStatusCode)
                        return false;
                    else return true;
            }
            catch
            {
                return false;
            }
        }

    }
}
