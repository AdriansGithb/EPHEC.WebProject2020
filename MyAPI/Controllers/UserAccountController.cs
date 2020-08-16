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

        /// <summary>
        /// Get all user accounts
        /// </summary>
        /// <returns>List<UserAccountAdministrationVwMdl></returns>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [Route("~/UserAccounts/GetAll")]
        public IActionResult GetAllUserAccounts()
        {
            try
            {
                var allUserAccounts = _service.GetAll();
                return Ok(allUserAccounts);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }

        /// <summary>
        /// Get one user account by Id
        /// </summary>
        /// <param name="id">User id</param>
        /// <returns>UserAccountVwMdl</returns>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
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

        /// <summary>
        /// Delete a user account by user id
        /// </summary>
        /// <param name="id">user id</param>
        /// <returns></returns>
        [HttpDelete]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [Route("~/UserAccounts/Delete/{id}")]
        public IActionResult Delete(string id)
        {
            try
            {
                var deletion = _service.DeleteUserAccount(id);
                if (deletion.Equals("success"))
                    return Ok();
                else return BadRequest(deletion);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }

    }
}
