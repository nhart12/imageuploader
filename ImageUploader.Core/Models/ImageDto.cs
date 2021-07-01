using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace ImageUploader.Core.Models
{
    public class ImageDto
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public IEnumerable<string> Tags { get; set; }
        public string Path { get; set; }
        public string FileName { get; set; }
        [JsonIgnore]
        public string ContentType { get; set; }
        [JsonIgnore]
        public byte[] ImageData { get; set; }
    }

    public class ImageDtoValidator: AbstractValidator<ImageDto>
    {
        public ImageDtoValidator()
        {
            RuleFor(x => x.Title).NotEmpty();
            RuleFor(x => x.Description).NotEmpty();
            RuleFor(x => x.Tags).NotEmpty();
        }
    }
}
