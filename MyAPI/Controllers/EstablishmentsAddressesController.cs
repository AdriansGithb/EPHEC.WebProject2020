using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MyAPI.Services.Interfaces;
using MyLibrary.DTOs;

namespace MyAPI.Controllers
{
    [Produces("application/json")]
    [Route("~/EstablishmentsAddresses")]
    [ApiController]
    public class EstablishmentsAddressesController : ControllerBase
    {
        private readonly IEstablishmentsAddressesService _service;

        public EstablishmentsAddressesController(IEstablishmentsAddressesService service)
        {
            _service = service;
        }

        /// <summary>
        /// Get all establishment addresses
        /// </summary>
        /// <returns>List<AddressDTO></returns>
        [HttpGet]
        [AllowAnonymous]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [Route("~/EstablishmentsAddresses/GetAll")]
        public IActionResult GetAll()
        {
            try
            {
                List<AddressDTO> fullList = _service.GetAll();
                return Ok(fullList);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }

        /// <summary>
        /// Get only open now establishment addresses
        /// </summary>
        /// <returns>List<AddressDTO></returns>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [Route("~/EstablishmentsAddresses/GetAllOpen")]
        public IActionResult GetAllOpen()
        {
            try
            {
                List<AddressDTO> openList = _service.GetAllOpen();
                return Ok(openList);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }

        

    }
}
