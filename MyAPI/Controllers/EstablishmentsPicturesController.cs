using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MyAPI.Services.Interfaces;
using MyLibrary.DTOs;
using MyLibrary.ViewModels;

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

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [Route("~/EstablishmentsPictures/GetPicture/{id}")]
        public IActionResult GetPicture(string id)
        {
            try
            {
                PicturesDTO pic = _service.GetPicture(id);
                return Ok(pic);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [Route("~/EstablishmentsPictures/GetCurrentPictures/{id}")]
        public IActionResult GetCurrentPictures(int id)
        {
            try
            {
                EstablishmentPicturesEditionVwMdl estabPics = _service.GetCurrentPictures(id);
                return Ok(estabPics);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }

        [HttpPut]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [Route("~/EstablishmentsPictures/EditPictures")]
        public IActionResult EditPictures(EstablishmentPicturesEditionVwMdl estabPics)
        {
            try
            {
                _service.EditPictures(estabPics);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }

    }
}


    
