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

    }
}
