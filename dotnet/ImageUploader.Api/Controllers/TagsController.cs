using ImageUploader.Core;
using Microsoft.AspNetCore.Mvc;
using System;

namespace ImageUploader.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TagsController : ControllerBase
    {
        private readonly IImageUploadService imageUploadService;
        public TagsController(IImageUploadService imageUploadService)
        {
            this.imageUploadService = imageUploadService ?? throw new ArgumentNullException(nameof(imageUploadService));
        }

        [HttpGet]
        public IActionResult GetAllImageTags()
        {
            return Ok(imageUploadService.GetAllTags());
        }
    }
}
