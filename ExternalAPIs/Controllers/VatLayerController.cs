using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ExternalAPIs.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ExternalAPIs.Controllers
{
    [Route("~/VatLayer")]
    [ApiController]
    public class VatLayerController : ControllerBase
    {
        private readonly IVatLayerService _service;
        public VatLayerController(IVatLayerService service)
        {
            _service = service;
        }

        [HttpGet]
        [Route("~/VatLayer/{vatnumber}")]
        public async Task<IActionResult> GetVatLayerResponseAsync(string vatnumber)
        {
            try
            {
                var content = await _service.GetVatLayerResponseAsync(vatnumber);
                return Ok(content);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }
    }
}
