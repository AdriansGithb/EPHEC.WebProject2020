using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MyAPI.Services.Interfaces;

namespace MyAPI.Controllers
{
    [Produces("application/json")]
    [Route("~/EstablishmentsAddresses")]
    [ApiController]
    public class EstablishmentsAddressesController : ControllerBase
    {
        private readonly IEstablishmentsAddressesService _service;

        public EstablishmentsAddressesController(IEstablishmentsAddressesService service)
        {
            _service = service;
        }
        
    }
}
