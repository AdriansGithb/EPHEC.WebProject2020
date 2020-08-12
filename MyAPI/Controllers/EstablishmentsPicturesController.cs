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
    [Route("~/EstablishmentsPictures")]
    [ApiController]
    public class EstablishmentsPicturesController : ControllerBase
    {
        private readonly IEstablishmentsPicturesService _service;

        public EstablishmentsPicturesController(IEstablishmentsPicturesService service)
        {
            _service = service;
        }

        [HttpGet]
        [AllowAnonymous]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [Route("~/EstablishmentsPictures/GetLogo/{id}")]
        public IActionResult GetLogo(int id)
        {
            try
            {
                PicturesDTO logo = _service.GetLogo(id);
                return Ok(logo);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }
    }
}


    
