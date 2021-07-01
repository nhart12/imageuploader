using ImageUploader.Core;
using ImageUploader.Core.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;

namespace ImageUploader.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ImagesController : ControllerBase
    {
        private readonly IImageUploadService imageUploadService;
        public ImagesController(IImageUploadService imageUploadService)
        {
            this.imageUploadService = imageUploadService ?? throw new ArgumentNullException(nameof(imageUploadService));
        }

        [HttpGet("{id}/{fileName}")]
        public IActionResult DownloadImage(Guid id, string fileName)
        {
            var img = imageUploadService.Get(id);
            if(img?.FileName != fileName) { return NotFound(); }
            return File(img.ImageData, img.ContentType, img.FileName);
        }

        [HttpPost]
        public IActionResult UploadImage([FromForm] IFormFile file)
        {
            using var memStream = new MemoryStream();
            file.CopyTo(memStream);
            return Ok(imageUploadService.UploadImage(file.FileName, file.ContentType, memStream.ToArray()));
        }

        [HttpPost("{id}/metadata")]
        public IActionResult SetImageMetadata(Guid id, [FromBody] ImageDto img)
        {
            return Ok(imageUploadService.SetImageMetadata(id, img));
        }

        [HttpGet("{id}")]
        public IActionResult GetImage(Guid id)
        {
            return Ok(imageUploadService.Get(id));
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteImage(Guid id)
        {
            imageUploadService.DeleteImage(id);
            return Ok();
        }

        [HttpGet("search")]
        public IActionResult SearchImage([FromQuery] IEnumerable<string> tags)
        {
            return Ok(imageUploadService.GetByTags(tags));
        }

        [HttpGet]
        public IActionResult GetAllImages()
        {
            return Ok(imageUploadService.GetAll());
        }
        
    }
}
