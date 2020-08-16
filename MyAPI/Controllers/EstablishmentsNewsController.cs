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
        [Route("~/EstablishmentsNews/Get/{id}")]
        public IActionResult Get(string id)
        {
            try
            {
                EstablishmentNewsVwMdl news = _service.Get(id);
                return Ok(news);
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

        [HttpPut]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [Route("~/EstablishmentsNews/Edit")]
        public IActionResult Edit(EstablishmentNewsVwMdl news)
        {
            try
            {
                _service.Edit(news);
                return Ok();
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
        [Route("~/EstablishmentsNews/Delete/{id}")]
        public IActionResult Delete(string id)
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

        [HttpGet]
        [AllowAnonymous]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [Route("~/EstablishmentsNews/GetLastNews")]
        public IActionResult GetLastNews()
        {
            try
            {
                List<EstablishmentNewsVwMdl> lastNews = _service.GetLastNews();
                return Ok(lastNews);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }

        
    }
}
