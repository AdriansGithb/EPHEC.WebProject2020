using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;


namespace MVCClient.Controllers
{
    public class BaseController : Controller
    {
        /// <summary>
        /// Add success message for display in TempData
        /// </summary>
        /// <param name="message"></param>
        protected void AddSuccessMessage(string title, string message)
        {
            AddTempDataStrings(title, message, "Success");
        }

        /// <summary>
        /// Add info message for display in TempData
        /// </summary>
        /// <param name="message"></param>
        protected void AddInfoMessage(string title, string message)
        {
            AddTempDataStrings(title, message, "Info");
        }

        /// <summary>
        /// Add warning message for display in TempData
        /// </summary>
        /// <param name="message"></param>
        protected void AddWarningMessage(string title, string message)
        {
            AddTempDataStrings(title, message, "Warning");
        }

        /// <summary>
        /// Add error message for display in TempData
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
            TempData["TstrTitle"] = title;
            TempData["TstrMsg"] = message;
            TempData["TstrType"] = type;
        }
    }
}
