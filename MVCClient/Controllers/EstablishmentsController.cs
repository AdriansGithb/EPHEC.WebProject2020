using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MyLibrary.Constants;
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

        //// GET: EstablishmentsController
        //public ActionResult Index()
        //{
        //    return View();
        //}

        //// GET: EstablishmentsController/Details/5
        //public ActionResult Details(int id)
        //{
        //    return View();
        //}

        // GET: EstablishmentsController/Create
        [Authorize(Roles = MyIdentityServerConstants.Role_Admin_Manager)]
        public ActionResult Create()
        {
            AddCountryListData();
            return View();
        }

        // POST: EstablishmentsController/Create
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
