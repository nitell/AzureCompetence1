using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using System.Net.Http;
using AzureCompetence1.Storage;
using Microsoft.ApplicationInsights.AspNetCore.Extensions;

namespace AzureCompetence1.Controllers
{
    [Route("api/[controller]")]
    public class ImagesController : Controller
    {
        private readonly IImageStorage storage;

        public ImagesController(IImageStorage storage)
        {
            this.storage = storage;
        }

        [HttpGet()]
        public async Task<IActionResult> Get()
        {
            return Ok(storage.Images);            
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(string imageName)
        {
            return Ok();
        }

        [HttpPost("{imageName}")]
        public async Task<IActionResult> Post(string imageName)
        {
            storage.Insert(imageName);
            return Created(Request.GetUri(), imageName);
        }
    }
}
