﻿using System;
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

    }
}
