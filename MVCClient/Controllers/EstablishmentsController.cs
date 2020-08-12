using System;
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

        [HttpGet]
        [AllowAnonymous]
        public async Task<ActionResult> Index(int pageNumber = 1, int pageSize = 2)
        {
            var accessToken = await HttpContext.GetTokenAsync("access_token");
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

            var httpResponse = await _client.GetAsync($"{MyAPIConstants.MyAPI_EstablishmentsCtrl_Url}GetAllValidated");
            if (!httpResponse.IsSuccessStatusCode)
            {
                AddErrorMessage("Data not downloaded", httpResponse.ReasonPhrase);
                return View("../Home/Index");
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

            ViewData["Title"] = "Establishments index";
            ViewData["Action"] = "Index";
            ViewData["HeadText"] = "List of all our establishments";

            return View("Index", pageResult);
        }

        [HttpGet]
        [Authorize(Roles = MyIdentityServerConstants.Role_Admin_Manager)]
        public async Task<ActionResult> GetAllByManager(int pageNumber = 1, int pageSize = 2)
        {
            var accessToken = await HttpContext.GetTokenAsync("access_token");
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

            var httpResponse = await _client.GetAsync($"{MyAPIConstants.MyAPI_EstablishmentsCtrl_Url}GetAllByManager/{User.Claims.First(x=>x.Type.Contains("sub")).Value}");
            if (!httpResponse.IsSuccessStatusCode)
            {
                AddErrorMessage("Data not downloaded", httpResponse.ReasonPhrase);
                return View("../Home/Index");
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


        [HttpGet]
        [Authorize(Roles = MyIdentityServerConstants.Role_Admin_Manager)]
        public async Task<ActionResult> Details(int id)
        {
            var accessToken = await HttpContext.GetTokenAsync("access_token");
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

            var httpResponse = await _client.GetAsync($"{MyAPIConstants.MyAPI_EstablishmentsCtrl_Url}GetDetails/{id}");
            if (!httpResponse.IsSuccessStatusCode)
            {
                AddErrorMessage("Error", httpResponse.ReasonPhrase);
                return View("../Home/Index");
            }

            var content = await httpResponse.Content.ReadAsStringAsync();
            EstablishmentFullVwMdl estabDetails = JsonConvert.DeserializeObject<EstablishmentFullVwMdl>(content);

            return View(estabDetails);
        }

        [HttpGet]
        [Authorize(Roles = MyIdentityServerConstants.Role_Admin_Manager)]
        public ActionResult Create()
        {
            AddCountryListData();
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = MyIdentityServerConstants.Role_Admin_Manager)]
        public async Task<ActionResult> Create(EstablishmentCreationVwMdl model)
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
                    return View("../Home/Index");

                }
                else
                {
                    AddCountryListData();
                    return View(model);
                }
            }
            catch
            {
                return View();
            }
        }

        [HttpGet]
        [Authorize(Roles = MyIdentityServerConstants.Role_Admin)]
        public async Task<ActionResult> GetAllNotValidated(int pageNumber = 1, int pageSize = 2)
        {
            var accessToken = await HttpContext.GetTokenAsync("access_token");
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

            var httpResponse = await _client.GetAsync($"{MyAPIConstants.MyAPI_EstablishmentsCtrl_Url}GetAllNotValidated");
            if (!httpResponse.IsSuccessStatusCode)
            {
                AddErrorMessage("Data not downloaded", httpResponse.ReasonPhrase);
                return View("../Home/Index");
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



        //// GET: EstablishmentsController/Edit/5
        //public ActionResult Edit(int id)
        //{
        //    return View();
        //}

        //// POST: EstablishmentsController/Edit/5
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public ActionResult Edit(int id, IFormCollection collection)
        //{
        //    try
        //    {
        //        return RedirectToAction(nameof(Index));
        //    }
        //    catch
        //    {
        //        return View();
        //    }
        //}

        //// GET: EstablishmentsController/Delete/5
        //public ActionResult Delete(int id)
        //{
        //    return View();
        //}

        //// POST: EstablishmentsController/Delete/5
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public ActionResult Delete(int id, IFormCollection collection)
        //{
        //    try
        //    {
        //        return RedirectToAction(nameof(Index));
        //    }
        //    catch
        //    {
        //        return View();
        //    }
        //}
        [HttpGet]
        public async Task<ActionResult> RenderLogo(int estabId)
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
                }
                else AddErrorMessage("Download image problem","An unknown error has occured during downloading the logo for establishment id: "+estabId);
                
                byte[] defaultLogo = GetDefaultPictureFromFile("~\\..\\Images\\defaultLogo.jpg");

                return File(defaultLogo, "image/jpg");
            }
            catch (Exception)
            {
                byte[] defaultLogo = GetDefaultPictureFromFile("~\\..\\Images\\defaultLogo.jpg");
                return File(defaultLogo, "image/jpg");
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
