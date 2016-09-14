using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using System.Net.Http;

namespace AzureCompetence1.Controllers
{
    [Route("api/[controller]")]
    public class ImagesController : Controller
    {        
        [HttpGet()]
        public async Task<IActionResult> Get()
        {
            return Ok(new[] {"Kalle", "Nisse"});
        }
     
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(string imageName)
        {
            return Ok();
        }

        [HttpPost]
        public async Task<IActionResult> Post()
        {
            return Ok();
        }         
    }
}
