using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MyAPI.Data;
using MyAPI.Services.Interfaces;
using MyLibrary.Entities;
using MyLibrary.ViewModels;
using Newtonsoft.Json;

namespace MyAPI.Controllers
{
    [Produces("application/json")]
    [Route("~/UserAccounts")]
    [ApiController]
    public class UserAccountController : ControllerBase
    {
        private readonly IUserAccountService _service;

        public UserAccountController(IUserAccountService service)
        {
            _service = service;
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [Route("~/UserAccounts/GetAll")]
        public IActionResult GetAllUserAccounts()
        {
            var allUserAccounts = _service.GetAll();
            return Ok(allUserAccounts);
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [Route("~/UserAccounts/{id}")]
        public IActionResult GetUserAccount(string id)
        {
            try
            {
                var userAccount = _service.GetUserAccount(id);
                return Ok(userAccount);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }

    }
}
