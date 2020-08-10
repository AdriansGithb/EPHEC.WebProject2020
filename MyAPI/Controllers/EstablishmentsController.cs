﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MyAPI.Services.Interfaces;
using MyLibrary.Entities;
using MyLibrary.ViewModels;

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

    }
}