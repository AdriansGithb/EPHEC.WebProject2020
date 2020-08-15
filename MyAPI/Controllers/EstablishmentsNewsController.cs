using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MyAPI.Services.Interfaces;
using MyLibrary.ViewModels;

namespace MyAPI.Controllers
{
    [Produces("application/json")]
    [Route("~/EstablishmentsNews")]
    [ApiController]
    public class EstablishmentsNewsController : ControllerBase
    {
        private readonly IEstablishmentsNewsService _service;
        public EstablishmentsNewsController(IEstablishmentsNewsService service)
        {
            _service = service;
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [Route("~/EstablishmentsNews/GetAll")]
        public IActionResult GetAll()
        {
            try
            {
                var allNews = _service.GetAll();
                return Ok(allNews);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [Route("~/EstablishmentsNews/Create")]
        public IActionResult Create(EstablishmentNewsVwMdl news)
        {
            try
            {
                _service.Create(news);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }

    }
}
