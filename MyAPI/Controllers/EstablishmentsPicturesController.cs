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

        /// <summary>
        /// Get an establishment logo from the establishment id
        /// </summary>
        /// <param name="id">establishment id</param>
        /// <returns>PicturesDTO</returns>
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

        /// <summary>
        /// Get a picture from the picture id
        /// </summary>
        /// <param name="id">picture id</param>
        /// <returns>PicturesDTO</returns>
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

        /// <summary>
        /// Get current pictures in db, of an establishment, from establishment id
        /// </summary>
        /// <param name="id">establishment id (int)</param>
        /// <returns>EstablishmentPicturesEditionVwMdl</returns>
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

        /// <summary>
        /// Add new pictures for an establishment. If a logo is sent, it errases and replace current logo if existent.
        /// </summary>
        /// <param name="estabPics">EstablishmentPicturesEditionVwMdl</param>
        /// <returns></returns>
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

        /// <summary>
        /// Delete all pictures of en establishment, from the establishment id
        /// </summary>
        /// <param name="id">int / establishment id</param>
        /// <returns></returns>
        [HttpDelete]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [Route("~/EstablishmentsPictures/DeletePictures/{id}")]
        public IActionResult DeletePictures(int id)
        {
            try
            {
                _service.DeletePictures(id);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }

    }
}


    
