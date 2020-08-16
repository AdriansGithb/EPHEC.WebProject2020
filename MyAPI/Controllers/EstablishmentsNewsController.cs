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

        /// <summary>
        /// Get a news, by id
        /// </summary>
        /// <param name="id">string</param>
        /// <returns></returns>
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

        /// <summary>
        /// Get all news
        /// </summary>
        /// <returns>List<NewsDTO></returns>
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

        /// <summary>
        /// Create a news
        /// </summary>
        /// <param name="news">EstablishmentNewsVwMdl</param>
        /// <returns></returns>
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

        /// <summary>
        /// Edit a news
        /// </summary>
        /// <param name="news">EstablishmentNewsVwMdl</param>
        /// <returns></returns>
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

        /// <summary>
        /// Delete a news, by id
        /// </summary>
        /// <param name="id">string</param>
        /// <returns></returns>
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

        /// <summary>
        /// Get the lastest news (max 10)
        /// </summary>
        /// <returns>List<EstablishmentNewsVwMdl></returns>
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
