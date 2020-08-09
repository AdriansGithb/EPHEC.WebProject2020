using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MyLibrary.Entities;
using MyLibrary.ViewModels;

namespace MVCClient.Controllers
{
    public class EstablishmentsController : BaseController
    {
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
        public ActionResult Create()
        {
            AddCountryListData();
            return View();
        }

        // POST: EstablishmentsController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(EstablishmentCreationVwMdl model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var test = model;
                    return RedirectToAction();

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
