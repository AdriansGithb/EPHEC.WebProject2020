using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using System.Web;
using MyLibrary.Cookies;

namespace MVCClient.Controllers
{
    public class BaseController : Controller
    {
        /// <summary>
        /// Check if ToastrCookie exists, if it exists, add message to TempData
        /// </summary>
        protected void CheckIfToastrCookieAndShowIt()
        {
            if (HttpContext.Request.Cookies.ContainsKey(MyCookies.ToastrMessage))
            {
                string title = HttpContext.Request.Cookies[MyCookies.ToastrTitle];
                string msg = HttpContext.Request.Cookies[MyCookies.ToastrMessage];
                string type = HttpContext.Request.Cookies[MyCookies.ToastrType];
                switch (type)
                {
                    case "Success":
                        AddSuccessMessage(title, msg);
                        break;
                    case "Info":
                        AddInfoMessage(title, msg);
                        break;
                    case "Warning":
                        AddWarningMessage(title, msg);
                        break;
                    case "Error":
                        AddErrorMessage(title, msg);
                        break;
                }
                HttpContext.Response.Cookies.Delete(MyCookies.ToastrTitle);
                HttpContext.Response.Cookies.Delete(MyCookies.ToastrMessage);
                HttpContext.Response.Cookies.Delete(MyCookies.ToastrType);
            }

        }

        /// <summary>
        /// Add success message to display in TempData
        /// </summary>
        /// <param name="message"></param>
        protected void AddSuccessMessage(string title, string message)
        {
            AddTempDataStrings(title, message, "Success");
        }

        /// <summary>
        /// Add info message to display in TempData
        /// </summary>
        /// <param name="message"></param>
        protected void AddInfoMessage(string title, string message)
        {
            AddTempDataStrings(title, message, "Info");
        }

        /// <summary>
        /// Add warning message to display in TempData
        /// </summary>
        /// <param name="message"></param>
        protected void AddWarningMessage(string title, string message)
        {
            AddTempDataStrings(title, message, "Warning");
        }

        /// <summary>
        /// Add error message to display in TempData
        /// </summary>
        /// <param name="message"></param>
        protected void AddErrorMessage(string title, string message)
        {
            AddTempDataStrings(title, message, "Error");
        }

        /// <summary>
        /// Add toastr needed fields in TempData
        /// </summary>
        /// <param name="title"></param>
        /// <param name="message"></param>
        /// <param name="type"></param>
        private void AddTempDataStrings(string title, string message, string type)
        {
            TempData[MyCookies.ToastrTitle] = title;
            TempData[MyCookies.ToastrMessage] = message;
            TempData[MyCookies.ToastrType] = type;
        }
    }
}
