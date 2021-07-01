using ImageUploader.Core.Models;
using System;
using System.Collections.Generic;

namespace ImageUploader.Core
{
    public interface IImageUploadService
    {
        public Guid UploadImage(string fileName, string contentType, byte[] imgData);
        public ImageDto SetImageMetadata(Guid id, ImageDto imageDto);
        public void DeleteImage(Guid id);
        public ImageDto Get(Guid id);
        public IEnumerable<ImageDto> GetAll();
        public IEnumerable<ImageDto> GetByTags(IEnumerable<string> tags);
        public IEnumerable<ImageDto> GetByTitle(string title);
    }
}
