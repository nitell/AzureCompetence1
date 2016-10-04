using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Mime;
using AzureCompetence1.Storage;
using Microsoft.ApplicationInsights.AspNetCore.Extensions;
using Microsoft.AspNetCore.Http;

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
            return Ok(storage.Images.Select(i => Url.Action("Get", new { imageName = i })));
        }

        [HttpGet("{imageName}")]
        public FileStreamResult Get(string imageName)
        {
            return Get(imageName, "regular");
        }

        [HttpGet("{imageName}({imageSize}")]
        public FileStreamResult Get(string imageName, string imageSize)
        {
            var img = storage.GetImage(imageName, imageSize);                     
            return new FileStreamResult(new MemoryStream(img), "image/png");
        }

        [HttpPost]
        public async Task<IActionResult> Post()
        {
            foreach (var f in HttpContext.Request.Form.Files)
            {
                var memStream = new MemoryStream();
                f.OpenReadStream().CopyTo(memStream);
                storage.Insert(f.FileName, memStream.ToArray());
            }
            return Ok();
        }
    }
}
