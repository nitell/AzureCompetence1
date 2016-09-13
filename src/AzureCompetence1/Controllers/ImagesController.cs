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
        public string[] Get()
        {
            return new[] {"Nisse", "Kalle"};
        }
     
        [HttpGet("{id}")]
        public HttpResponseMessage Get(string imageName)
        {
            return null;
        }

        [HttpPost]
        public void Post()
        {
        }         
    }
}
