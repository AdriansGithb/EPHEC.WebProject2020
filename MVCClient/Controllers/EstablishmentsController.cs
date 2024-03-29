﻿using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Mime;
using System.Text;
using System.Threading.Tasks;
using cloudscribe.Pagination.Models;
using FluentValidation.TestHelper;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Hosting.Internal;
using MyLibrary.Constants;
using MyLibrary.DTOs;
using MyLibrary.Entities;
using MyLibrary.ViewModels;
using Newtonsoft.Json;


namespace MVCClient.Controllers
{
    public class EstablishmentsController : BaseController
    {
        private readonly HttpClient _client;
        public EstablishmentsController()
        {
            _client = new HttpClient();
        }

        /// <summary>
        /// Routing for shorten url
        /// </summary>
        /// <param name="urlToken">token</param>
        /// <returns></returns>
        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> FromShortUrl(string urlToken)
        {
            try
            {
                var accessToken = await HttpContext.GetTokenAsync("access_token");
                _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

                var httpResponse = await _client.GetAsync($"{MyAPIConstants.MyAPI_EstablishmentsCtrl_Url}GetIdFromShortUrl/{urlToken}");
                if (!httpResponse.IsSuccessStatusCode)
                {
                    AddErrorMessage("Error", httpResponse.ReasonPhrase);
                    return RedirectToAction("Index");
                }

                var content = await httpResponse.Content.ReadAsStringAsync();
                int estabId = JsonConvert.DeserializeObject<int>(content);

                return RedirectToAction("Details", new {id = estabId});
            }
            catch (Exception e)
            {
                AddErrorMessage("Unknown error",e.Message);
                return RedirectToAction("Index", "Home");
            }
        }

        /// <summary>
        /// List all the validated establishments, as short viewed
        /// </summary>
        /// <param name="pageNumber"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> Index(int pageNumber = 1, int pageSize = 3)
        {
            try
            {
                var accessToken = await HttpContext.GetTokenAsync("access_token");
                _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

                var httpResponse =
                    await _client.GetAsync($"{MyAPIConstants.MyAPI_EstablishmentsCtrl_Url}GetAllValidated");
                if (!httpResponse.IsSuccessStatusCode)
                {
                    AddErrorMessage("Data not downloaded", httpResponse.ReasonPhrase);
                    return RedirectToAction("Index", "Home");
                }

                var content = await httpResponse.Content.ReadAsStringAsync();
                List<EstablishmentShortVwMdl> displayList =
                    JsonConvert.DeserializeObject<List<EstablishmentShortVwMdl>>(content);

                int excludeRecords = (pageSize * pageNumber) - pageSize;
                var paginatedList = displayList.Skip(excludeRecords).Take(pageSize);
                var pageResult = new PagedResult<EstablishmentShortVwMdl>
                {
                    Data = paginatedList.ToList(),
                    TotalItems = displayList.Count(),
                    PageNumber = pageNumber,
                    PageSize = pageSize
                };

                ViewData["Title"] = "Establishments index";
                ViewData["Action"] = "Index";
                ViewData["HeadText"] = "List of all our establishments";

                return View("Index", pageResult);
            }

            catch (Exception ex)
            {
                AddErrorMessage("Unknown error", ex.Message);
                return RedirectToAction("Index","Home");
            }

        }

        [HttpGet]
        [Authorize(Roles = MyIdentityServerConstants.Role_Admin_Manager)]
        public async Task<IActionResult> GetAllByManager(int pageNumber = 1, int pageSize = 2)
        {
            try
            {
            var accessToken = await HttpContext.GetTokenAsync("access_token");
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

            var httpResponse = await _client.GetAsync($"{MyAPIConstants.MyAPI_EstablishmentsCtrl_Url}GetAllByManager/{User.Claims.First(x=>x.Type.Contains("sub")).Value}");
            if (!httpResponse.IsSuccessStatusCode)
            {
                AddErrorMessage("Data not downloaded", httpResponse.ReasonPhrase);
                return RedirectToAction("Index", "Home");
            }

            var content = await httpResponse.Content.ReadAsStringAsync();
            List<EstablishmentShortVwMdl> displayList = JsonConvert.DeserializeObject<List<EstablishmentShortVwMdl>>(content);

            int excludeRecords = (pageSize * pageNumber) - pageSize;
            var paginatedList = displayList.Skip(excludeRecords).Take(pageSize);
            var pageResult = new PagedResult<EstablishmentShortVwMdl>
            {
                Data = paginatedList.ToList(),
                TotalItems = displayList.Count(),
                PageNumber = pageNumber,
                PageSize = pageSize
            };

            ViewData["Title"] = "Establishments by manager";
            ViewData["Action"] = "GetAllByManager";
            ViewData["HeadText"] = "Your establishments";

            return View("Index", pageResult);
            }

            catch (Exception ex)
            {
                AddErrorMessage("Unknown error", ex.Message);
                return RedirectToAction("Index", "Home");
            }

        }

        [HttpGet]
        [Authorize(Roles = MyIdentityServerConstants.Role_Admin)]
        public async Task<IActionResult> GetAllForAdmin(int pageNumber = 1, int pageSize = 2)
        {
            try
            {
                var accessToken = await HttpContext.GetTokenAsync("access_token");
                _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

                var httpResponse =
                    await _client.GetAsync($"{MyAPIConstants.MyAPI_EstablishmentsCtrl_Url}GetAllForAdmin");
                if (!httpResponse.IsSuccessStatusCode)
                {
                    AddErrorMessage("Data not downloaded", httpResponse.ReasonPhrase);
                    return RedirectToAction("Index", "Home");
                }

                var content = await httpResponse.Content.ReadAsStringAsync();
                List<EstablishmentShortVwMdl> displayList =
                    JsonConvert.DeserializeObject<List<EstablishmentShortVwMdl>>(content);

                int excludeRecords = (pageSize * pageNumber) - pageSize;
                var paginatedList = displayList.Skip(excludeRecords).Take(pageSize);
                var pageResult = new PagedResult<EstablishmentShortVwMdl>
                {
                    Data = paginatedList.ToList(),
                    TotalItems = displayList.Count(),
                    PageNumber = pageNumber,
                    PageSize = pageSize
                };

                ViewData["Title"] = "Establishments for administration";
                ViewData["Action"] = "GetAllForAdmin";
                ViewData["HeadText"] = "All establishments";

                return View("Index", pageResult);
            
        }

        catch (Exception ex)
        {
            AddErrorMessage("Unknown error", ex.Message);
            return RedirectToAction("Index","Home");
        }

}

        [HttpGet]
        [Authorize(Roles = MyIdentityServerConstants.Role_Admin_Manager_User)]
        [Route("~/Establishments/Details/{id}")]
        public async Task<IActionResult> Details(int id)
        {
            try
            {
                var accessToken = await HttpContext.GetTokenAsync("access_token");
                _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

                var httpResponse =
                    await _client.GetAsync($"{MyAPIConstants.MyAPI_EstablishmentsCtrl_Url}GetDetails/{id}");
                if (!httpResponse.IsSuccessStatusCode)
                {
                    AddErrorMessage("Error", httpResponse.ReasonPhrase);
                    return RedirectToAction("Index");
                }

                var content = await httpResponse.Content.ReadAsStringAsync();
                EstablishmentFullVwMdl estabDetails = JsonConvert.DeserializeObject<EstablishmentFullVwMdl>(content);

                return View(estabDetails);

            }
            catch (Exception ex)
            {
                AddErrorMessage("Error",ex.Message);
                return RedirectToAction("Index");
            }
        }

        [HttpGet]
        [Authorize(Roles = MyIdentityServerConstants.Role_Admin_Manager)]
        public IActionResult Create()
        {
            AddCountryListData();
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = MyIdentityServerConstants.Role_Admin_Manager)]
        public async Task<IActionResult> Create(EstablishmentCreationVwMdl model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    Establishments newEstab = new Establishments
                    {
                        Id = 0,
                        Name = model.Name,
                        VatNum = model.VatNum,
                        Email = model.Email,
                        Description = model.Description,
                        IsValidated = false,
                        ManagerId = model.ManagerId,
                        TypeId = model.TypeId,
                        Details = model.Details,
                        Address = model.Address,
                        OpeningTimes = model.OpeningTimes
                    };


                    newEstab.Pictures = await SetUploadPicturesList(model.Logo.PictureFile,model.Pictures);

                    var accessToken = await HttpContext.GetTokenAsync("access_token");
                    _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

                    var postContent = JsonConvert.SerializeObject(newEstab);
                    var httpContent = new StringContent(postContent, Encoding.Default,"application/json");
                    var httpResponse = await _client.PostAsync($"{MyAPIConstants.MyAPI_EstablishmentsCtrl_Url}Create",httpContent);

                    if (!httpResponse.IsSuccessStatusCode)
                    {
                        AddErrorMessage("Not created", "Your new establishment has not been saved due to some issues. Please try again or contact an admin");
                        AddCountryListData();
                        return View(model);
                    }

                    AddSuccessMessage("Establishment created", "Your new establishment has been created successfully");
                    return RedirectToAction("Index", "Home");

                }
                else
                {
                    AddCountryListData();
                    return View(model);
                }
            }
            catch(Exception ex)
            {
                AddErrorMessage("Unknown error", ex.Message);
                return RedirectToAction("Index", "Home"); ;
            }
        }

        [HttpGet]
        [Authorize(Roles=MyIdentityServerConstants.Role_Admin_Manager)]
        public async Task<IActionResult> ShortenUrl(int estabId, string estabName)
        {
            try
            {
                var accessToken = await HttpContext.GetTokenAsync("access_token");
                _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

                var httpResponse = await _client.GetAsync($"{MyAPIConstants.MyAPI_EstablishmentsCtrl_Url}GetShortUrl/{estabId}");
                if (!httpResponse.IsSuccessStatusCode)
                {
                    AddErrorMessage("Error", httpResponse.ReasonPhrase);
                    return RedirectToAction("Details", new { id = estabId });
                }
                else
                {
                    var content = await httpResponse.Content.ReadAsStringAsync();
                    ShortenUrlVwMdl model = JsonConvert.DeserializeObject<ShortenUrlVwMdl>(content);
                    return View(model);
                }
            }
            catch (Exception e)
            {
                AddErrorMessage("Error", "Unknown error : "+e.Message);
                return RedirectToAction("Index", "Home");
            }
        }

        [HttpPost]
        [Authorize(Roles = MyIdentityServerConstants.Role_Admin_Manager)]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ShortenUrl(ShortenUrlVwMdl model)
        {
            try
            {
                var accessToken = await HttpContext.GetTokenAsync("access_token");
                _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

                var httpResponse = await _client.PutAsync(
                    $"{MyAPIConstants.MyAPI_EstablishmentsCtrl_Url}GenerateShortUrl/{model.EstablishmentId}", null);
                if (!httpResponse.IsSuccessStatusCode)
                {
                    AddErrorMessage("Error", httpResponse.ReasonPhrase);
                    return RedirectToAction("ShortenUrl",
                        new {estabId = model.EstablishmentId, estabName = model.EstabName});
                }
                else
                {
                    var content = await httpResponse.Content.ReadAsStringAsync();
                    ShortenUrlVwMdl editedMdl = JsonConvert.DeserializeObject<ShortenUrlVwMdl>(content);

                    AddSuccessMessage("Congratulations", "Your new short Url has been created and is operational.");
                    return View(editedMdl);
                }
            }
            catch (Exception ex)
            {
                AddErrorMessage("Unknown error", ex.Message);
                return View(model);
            }
        }

        [HttpGet]
        [Authorize(Roles = MyIdentityServerConstants.Role_Admin)]
        public async Task<IActionResult> GetAllNotValidated(int pageNumber = 1, int pageSize = 2)
        {
            var accessToken = await HttpContext.GetTokenAsync("access_token");
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

            var httpResponse = await _client.GetAsync($"{MyAPIConstants.MyAPI_EstablishmentsCtrl_Url}GetAllNotValidated");
            if (!httpResponse.IsSuccessStatusCode)
            {
                AddErrorMessage("Data not downloaded", httpResponse.ReasonPhrase);
                return RedirectToAction("Index", "Home"); ;
            }

            var content = await httpResponse.Content.ReadAsStringAsync();
            List<EstablishmentShortVwMdl> displayList = JsonConvert.DeserializeObject<List<EstablishmentShortVwMdl>>(content);
            
            int excludeRecords = (pageSize * pageNumber) - pageSize;
            var paginatedList = displayList.Skip(excludeRecords).Take(pageSize);
            var pageResult = new PagedResult<EstablishmentShortVwMdl>
            {
                Data = paginatedList.ToList(),
                TotalItems = displayList.Count(),
                PageNumber = pageNumber,
                PageSize = pageSize
            };

            ViewData["Title"] = "Establishments validation";
            ViewData["Action"] = "GetAllNotValidated";
            ViewData["HeadText"] = "Waiting validation establishments list";

            return View("Index",pageResult);
        }

        [HttpPost]
        public async Task<bool> Validate(int estabId)
        {
            try
            {
                var accessToken = await HttpContext.GetTokenAsync("access_token");
                _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

                var httpResponse = await _client.PutAsync($"{MyAPIConstants.MyAPI_EstablishmentsCtrl_Url}Validate/{estabId}",null);
                if (httpResponse.IsSuccessStatusCode)
                    return true;
                else return false;
            }
            catch
            {
                return false;
            }
        }

        [HttpGet]
        [Authorize(Roles = MyIdentityServerConstants.Role_Admin_Manager)]
        public async Task<IActionResult> Edit(int id)
        {
            try
            {
                var accessToken = await HttpContext.GetTokenAsync("access_token");
                _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

                var httpResponse = await _client.GetAsync($"{MyAPIConstants.MyAPI_EstablishmentsCtrl_Url}GetEstablishment/{id}");
                if (!httpResponse.IsSuccessStatusCode)
                {
                    AddErrorMessage("Error", httpResponse.ReasonPhrase);
                    return RedirectToAction("Index", "Home");
                }

                var content = await httpResponse.Content.ReadAsStringAsync();
                EstablishmentEditionVwMdl estab = JsonConvert.DeserializeObject<EstablishmentEditionVwMdl>(content);

                AddCountryListData();
                return View(estab);
            }
            catch (Exception ex)
            {
                AddErrorMessage("Page not loaded", "An unknown error has blocked the page loading : "+ex.Message);
                return RedirectToAction("Index", "Home");
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(EstablishmentEditionVwMdl model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var accessToken = await HttpContext.GetTokenAsync("access_token");
                    _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

                    var postContent = JsonConvert.SerializeObject(model);
                    var httpContent = new StringContent(postContent, Encoding.Default, "application/json");
                    var httpResponse = await _client.PutAsync($"{MyAPIConstants.MyAPI_EstablishmentsCtrl_Url}Edit",
                        httpContent);

                    if (!httpResponse.IsSuccessStatusCode)
                    {
                        AddErrorMessage("Not modified",
                            "Your modifications have not been saved due to some issues. Please try again or contact an admin");
                        AddCountryListData();
                        return View(model);
                    }

                    AddSuccessMessage("Establishment modified",
                        "Your establishment has been modified successfully. Please wait for admin validation.");
                    return RedirectToAction("Details", new {id = model.Id});

                }
                else
                {
                    AddCountryListData();
                    return View(model);
                }
            }
            catch (Exception ex)
            {
                AddErrorMessage("Unknown error",ex.Message);
                return RedirectToAction("Index", "Home");
            }
        }

        [HttpGet]
        [Authorize(Roles = MyIdentityServerConstants.Role_Admin_Manager)]
        public async Task<IActionResult> EditPictures(int estabId, string estabName)
        {
            try
            {
                var accessToken = await HttpContext.GetTokenAsync("access_token");
                _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

                var httpResponse = await _client.GetAsync($"{MyAPIConstants.MyAPI_EstabPicturesCtrl_Url}GetCurrentPictures/{estabId}");
                if (!httpResponse.IsSuccessStatusCode)
                {
                    AddErrorMessage("Error", httpResponse.ReasonPhrase);
                    return RedirectToAction("Index", "Home");
                }

                var content = await httpResponse.Content.ReadAsStringAsync();
                EstablishmentPicturesEditionVwMdl estabPictures = JsonConvert.DeserializeObject<EstablishmentPicturesEditionVwMdl>(content);
                estabPictures.EstabName = estabName;

                return View(estabPictures);
            }
            catch (Exception ex)
            {
                AddErrorMessage("Page not loaded", "An unknown error has blocked the page loading : " + ex.Message);
                return RedirectToAction("Index", "Home");
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditPictures(EstablishmentPicturesEditionVwMdl model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var accessToken = await HttpContext.GetTokenAsync("access_token");
                    _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

                    bool hasNewPicture = false;
                    foreach (PicturesEditionVwMdl oPic in model.NewPictures)
                    {
                        if (oPic.PictureFile != null)
                            hasNewPicture = true;
                    }

                    if (hasNewPicture)
                    {
                        for (int i = 0; i < model.NewPictures.Count; i++)
                        {
                            if (model.NewPictures[i].PictureFile != null)
                            {
                                model.NewPictures[i].PictureAsArray = await FormatPictureToArray(model.NewPictures[i].PictureFile);
                                model.NewPictures[i].PictureFile = null;
                            }
                            else
                            {
                                model.NewPictures.RemoveAt(i);
                                i--;
                            }
                        }
                        
                        var postContent = JsonConvert.SerializeObject(model);
                        var httpContent = new StringContent(postContent, Encoding.Default, "application/json");
                        var httpResponse = await _client.PutAsync($"{MyAPIConstants.MyAPI_EstabPicturesCtrl_Url}EditPictures", httpContent);

                        if (!httpResponse.IsSuccessStatusCode)
                        {
                            AddErrorMessage("Not modified",
                                "Your modifications have not been saved due to some issues. Please try again or contact an admin");
                            return View(model);
                        }

                        AddSuccessMessage("Establishment modified",
                            "Your establishment has been modified successfully. Please wait for admin validation.");
                        return RedirectToAction("Details", new{id = model.EstabId});
                    }
                    else
                    {
                        AddInfoMessage("No change to save", "You have sent an empty form.");
                        return View(model);
                    }
                }
                else
                {
                    return View(model);
                }
            }
            catch (Exception ex)
            {
                AddErrorMessage("Unknown error", ex.Message);
                return RedirectToAction("Index", "Home");
            }
        }

        [HttpPost]
        public async Task<IActionResult> DeletePictures(int estId, string estName)
        {
            try
            {
                var accessToken = await HttpContext.GetTokenAsync("access_token");
                _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

                var httpResponse =
                    await _client.DeleteAsync($"{MyAPIConstants.MyAPI_EstabPicturesCtrl_Url}DeletePictures/{estId}");

                if (!httpResponse.IsSuccessStatusCode)
                {
                    AddErrorMessage("Not deleted",
                        "Your pictures have not been deleted due to some issues. Please try again or contact an admin");
                    return RedirectToAction("EditPictures", new{estabId= estId, estabName= estName });
                }

                AddSuccessMessage("Pictures deleted",
                    "Your establishment pictures have been deleted successfully.");
                return RedirectToAction("EditPictures", new { estabId = estId, estabName = estName });
            }
            catch (Exception ex)
            {
                AddErrorMessage("Unknown error", ex.Message);
                return RedirectToAction("Index", "Home");
            }
        }

        [HttpPost]
        public async Task<bool> Delete(int estabId)
        {
            try
            {
                var accessToken = await HttpContext.GetTokenAsync("access_token");
                _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

                var httpResponse = await _client.DeleteAsync($"{MyAPIConstants.MyAPI_EstablishmentsCtrl_Url}Delete/{estabId}");
                if (httpResponse.IsSuccessStatusCode)
                    return true;
                else return false;
            }
            catch
            {
                return false;
            }
        }

        [HttpGet]
        [Authorize(Roles = MyIdentityServerConstants.Role_Admin_Manager_User)]
        public async Task<IActionResult> RenderPicture(string picId)
        {
            try
            {
                var accessToken = await HttpContext.GetTokenAsync("access_token");
                _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

                var httpResponse =
                    await _client.GetAsync($"{MyAPIConstants.MyAPI_EstabPicturesCtrl_Url}GetPicture/{picId}");
                if (httpResponse.IsSuccessStatusCode)
                {
                    var content = await httpResponse.Content.ReadAsStringAsync();
                    PicturesDTO pic = JsonConvert.DeserializeObject<PicturesDTO>(content);

                    if (pic.PictureAsArrayBytes != null)
                    {
                        string imgType = GetPictureFormatFromArrayFile(pic.PictureAsArrayBytes);
                        return File(pic.PictureAsArrayBytes, $"image/{imgType}");
                    }
                }
                else AddErrorMessage("Download image problem", "An unknown error has occured during downloading the picture id: " + picId + ": " + httpResponse.ReasonPhrase);

                byte[] notfoundLogo = GetDefaultPictureFromFile("~\\..\\Images\\notfound.jpg");
                return File(notfoundLogo, "image/jpg");
            }
            catch (Exception)
            {
                byte[] notfoundLogo = GetDefaultPictureFromFile("~\\..\\Images\\defaultLogo.jpg");
                return File(notfoundLogo, "image/jpg");
            }
        }
        
        [HttpGet]
        public async Task<IActionResult> RenderLogo(int estabId)
        {
            try
            {
                var accessToken = await HttpContext.GetTokenAsync("access_token");
                _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

                var httpResponse =
                    await _client.GetAsync($"{MyAPIConstants.MyAPI_EstabPicturesCtrl_Url}GetLogo/{estabId}");
                if (httpResponse.IsSuccessStatusCode)
                {
                    var content = await httpResponse.Content.ReadAsStringAsync();
                    PicturesDTO logo = JsonConvert.DeserializeObject<PicturesDTO>(content);

                    if (logo.PictureAsArrayBytes != null)
                    {
                        string imgType = GetPictureFormatFromArrayFile(logo.PictureAsArrayBytes);
                        return File(logo.PictureAsArrayBytes, $"image/{imgType}");
                    }
                    else
                    {
                        byte[] defaultLogo = GetDefaultPictureFromFile("~\\..\\Images\\defaultLogo.jpg");
                        return File(defaultLogo, "image/jpg");
                    }
                }
                else AddErrorMessage("Download image problem","An unknown error has occured during downloading the logo for establishment id: "+estabId+": "+httpResponse.ReasonPhrase);
                
                byte[] notfoundLogo = GetDefaultPictureFromFile("~\\..\\Images\\notfound.jpg");

                return File(notfoundLogo, "image/jpg");
            }
            catch (Exception)
            {
                byte[] notfoundLogo = GetDefaultPictureFromFile("~\\..\\Images\\defaultLogo.jpg");
                return File(notfoundLogo, "image/jpg");
            }
        }



        private byte[] GetDefaultPictureFromFile(string filePath)
        {
            using (var memoryStream = new MemoryStream())
            {
                Image tempImg = Image.FromFile("~\\..\\Images\\defaultLogo.jpg");
                //D:\\# Cours Informatique Ad\\Programmation côté client\\ProjetAout2eSess\\Code\\VS\\WebProject2020\\MVCClient
                tempImg.Save(memoryStream, ImageFormat.Jpeg);
                return memoryStream.ToArray();
            }

        }

        private async Task<List<EstablishmentsPictures>> SetUploadPicturesList(IFormFile logo, List<EstablishmentCreationPicturesVwMdl> picList)
        {
            List<EstablishmentsPictures> rtrnList = new List<EstablishmentsPictures>();

            if (logo != null)
            {
                EstablishmentsPictures oPic = new EstablishmentsPictures
                {
                    IsLogo = true,
                    Picture = await FormatPictureToArray(logo)
                };
                rtrnList.Add(oPic);
            }

            foreach (EstablishmentCreationPicturesVwMdl pic in picList)
            {
                if (pic.PictureFile != null)
                {
                    EstablishmentsPictures oPic = new EstablishmentsPictures
                    {
                        IsLogo = false,
                        Picture = await FormatPictureToArray(pic.PictureFile)
                    };
                    rtrnList.Add(oPic);
                }
            }

            return rtrnList;

        }

        private async Task<byte[]> FormatPictureToArray(IFormFile pic)
        {
            using (var memoryStream = new MemoryStream())
            {
                await pic.CopyToAsync(memoryStream);

                return memoryStream.ToArray();
            }

        }

        private string GetPictureFormatFromArrayFile(byte[] arrayPicture)
        {
            using (var memoryStream = new MemoryStream(arrayPicture))
            {
                Image test = Image.FromStream(memoryStream);
                return test.RawFormat.ToString();
            }

        }

        private void AddCountryListData()
        {
            List<string> CountryList = new List<string>();
            CultureInfo[] CInfoList = CultureInfo.GetCultures(CultureTypes.SpecificCultures);
            foreach (CultureInfo CInfo in CInfoList)
            {
                RegionInfo R = new RegionInfo(CInfo.LCID);
                if (!(CountryList.Contains(R.EnglishName)))
                {
                    CountryList.Add(R.EnglishName);
                }
            }

            CountryList.Sort();
            ViewBag.CountryList = CountryList;
        }

    }
}
