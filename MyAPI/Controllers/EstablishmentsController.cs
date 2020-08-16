using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MyAPI.Services.Interfaces;
using MyLibrary.DTOs;
using MyLibrary.Entities;
using MyLibrary.ViewModels;
using Newtonsoft.Json;

namespace MyAPI.Controllers
{
    [Produces("application/json")]
    [Route("~/Establishments")]
    [ApiController]
    public class EstablishmentsController : ControllerBase
    {        
        private readonly IEstablishmentsService _service;

        public EstablishmentsController(IEstablishmentsService service)
        {
            _service = service;
        }

        /// <summary>
        /// Create a new establishment
        /// </summary>
        /// <param name="newEstab">Establishments</param>
        /// <returns></returns>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [Route("~/Establishments/Create")]
        public IActionResult Create(Establishments newEstab)
        {
            var creation = _service.Create(newEstab);
            if (creation.Equals("success"))
                return Ok();
            else return BadRequest(creation);
        }

        /// <summary>
        /// Get an establishment, by id
        /// </summary>
        /// <param name="id">int</param>
        /// <returns></returns>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [Route("~/Establishments/GetEstablishment/{id}")]
        public IActionResult GetEstablishment(int id)
        {
            try
            {
                EstablishmentEditionVwMdl estab = _service.GetEstablishment(id);
                if (estab.Id != id)
                    return BadRequest("id not recognized");
                return Ok(estab);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }

        /// <summary>
        /// Get all establishments not yet validated by admin
        /// </summary>
        /// <returns>List<EstablishmentShortVwMdl></returns>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [Route("~/Establishments/GetAllNotValidated")]
        public IActionResult GetAllNotValidated()
        {
            try
            {
                List<EstablishmentShortVwMdl> rtrnList = _service.GetAllNotValidated();
                return Ok(rtrnList);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }

        /// <summary>
        /// Get all validated establishments
        /// </summary>
        /// <returns>List<EstablishmentShortVwMdl></returns>
        [HttpGet]
        [AllowAnonymous]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [Route("~/Establishments/GetAllValidated")]
        public IActionResult GetAllValidated()
        {
            try
            {
                List<EstablishmentShortVwMdl> rtrnList = _service.GetAllValidated();
                return Ok(rtrnList);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }

        /// <summary>
        /// Get all establishments (validated or not) owned by a manager, by id
        /// </summary>
        /// <param name="id">string</param>
        /// <returns>List<EstablishmentShortVwMdl></returns>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [Route("~/Establishments/GetAllByManager/{id}")]
        public IActionResult GetAllByManager(string id)
        {
            try
            {
                List<EstablishmentShortVwMdl> rtrnList = _service.GetAllByManager(id);
                return Ok(rtrnList);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }

        /// <summary>
        /// Get all establishments (validated or not), for administrators
        /// </summary>
        /// <returns>List<EstablishmentShortVwMdl></returns>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [Route("~/Establishments/GetAllForAdmin")]
        public IActionResult GetAllForAdmin()
        {
            try
            {
                List<EstablishmentShortVwMdl> rtrnList = _service.GetAllForAdmin();
                return Ok(rtrnList);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }

        /// <summary>
        /// Get an establishment with full related data, by establishment id
        /// </summary>
        /// <param name="id">int</param>
        /// <returns>EstablishmentFullVwMdl</returns>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [Route("~/Establishments/GetDetails/{id}")]
        public IActionResult GetDetails(int id)
        {
            try
            {
                EstablishmentFullVwMdl fullDetails = _service.GetDetails(id);
                if (fullDetails.Id != id)
                    return BadRequest("id not recognized");
                return Ok(fullDetails);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }

        /// <summary>
        /// Delete an establishment, by id
        /// </summary>
        /// <param name="id">int</param>
        /// <returns></returns>
        [HttpDelete]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [Route("~/Establishments/Delete/{id}")]
        public IActionResult Delete(int id)
        {
            try
            {
                _service.Delete(id);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }

        /// <summary>
        /// Validate an establishment in waiting validation, by establishment id
        /// (Set IsValid to true)
        /// </summary>
        /// <param name="id">int</param>
        /// <returns></returns>
        [HttpPut]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [Route("~/Establishments/Validate/{id}")]
        public IActionResult Validate(int id)
        {
            try
            {
                _service.Validate(id);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }

        /// <summary>
        /// Edit an establishment (with all related data, but no pictures)
        /// </summary>
        /// <param name="editedEstab">EstablishmentEditionVwMdl</param>
        /// <returns></returns>
        [HttpPut]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [Route("~/Establishments/Edit")]
        public IActionResult Edit(EstablishmentEditionVwMdl editedEstab)
        {
            try
            {
                _service.Edit(editedEstab);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// Get the registered shorten url of an establishment, by establishment id
        /// </summary>
        /// <param name="id">int</param>
        /// <returns>ShortenUrlVwMdl (ShortenUrlVwMdl.ShortUrl = null if never registered)</returns>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [Route("~/Establishments/GetShortUrl/{id}")]
        public IActionResult GetShortUrl(int id)
        {
            try
            {
                ShortenUrlVwMdl shortUrl = _service.GetShortUrl(id);
                if (shortUrl.EstablishmentId == id)
                    return Ok(shortUrl);
                else return BadRequest("Response establishment id is not equal to request establishment id");
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }

        /// <summary>
        /// Generate a valid and available short url, and save it in the db, by establishment id
        /// </summary>
        /// <param name="id">int</param>
        /// <returns>ShortenUrlVwMdl containing the generated short url</returns>
        [HttpPut]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [Route("~/Establishments/GenerateShortUrl/{id}")]
        public IActionResult GenerateShortUrl(int id)
        {
            try
            {
                ShortenUrlVwMdl shortUrl = _service.GenerateShortUrl(id);
                if (shortUrl.EstablishmentId == id)
                    return Ok(shortUrl);
                else return BadRequest("Response establishment id is not equal to request establishment id");
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }

        /// <summary>
        /// Get establishment id, from the token given in the short url
        /// </summary>
        /// <param name="urlToken">string</param>
        /// <returns>int</returns>
        [HttpGet]
        [AllowAnonymous]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [Route("~/Establishments/GetIdFromShortUrl/{urlToken}")]
        public IActionResult GetIdFromShortUrl(string urlToken)
        {
            try
            {
                int estabId = _service.GetIdFromShortUrl(urlToken);
                return Ok(estabId);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }

    }
}
