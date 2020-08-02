using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MyLibrary.Constants;
using MyLibrary.Cookies;

namespace IdentityServer.Controllers.Base
{
    public class BaseController : Controller
    {
        /// <summary>
        /// Add success message for display in TempData
        /// </summary>
        /// <param name="message"></param>
        protected void AddSuccessMessage(string title, string message)
        {
            AddToastrCookie(title, message, "Success");
        }

        /// <summary>
        /// Add info message for display in TempData
        /// </summary>
        /// <param name="message"></param>
        protected void AddInfoMessage(string title, string message)
        {
            AddToastrCookie(title, message, "Info");
        }

        /// <summary>
        /// Add warning message for display in TempData
        /// </summary>
        /// <param name="message"></param>
        protected void AddWarningMessage(string title, string message)
        {
            AddToastrCookie(title, message, "Warning");
        }

        /// <summary>
        /// Add error message for display in TempData
        /// </summary>
        /// <param name="message"></param>
        protected void AddErrorMessage(string title, string message)
        {
            AddToastrCookie(title, message, "Error");
        }

        /// <summary>
        /// Add toastr needed fields in TempData
        /// </summary>
        /// <param name="title"></param>
        /// <param name="message"></param>
        /// <param name="type"></param>
        private void AddToastrCookie(string title, string message, string type)
        {
            CookieOptions options = new CookieOptions();
            options.Secure = true;
            options.MaxAge = TimeSpan.FromMinutes(5.0);
            HttpContext.Response.Cookies.Append(MyCookies.ToastrTitle,title, options);
            HttpContext.Response.Cookies.Append(MyCookies.ToastrMessage, message, options);
            HttpContext.Response.Cookies.Append(MyCookies.ToastrType, type, options);
        }
    }
}
