using AutoMapper;
using ImageUploader.Core;
using ImageUploader.Core.Models;
using ImageUploader.Foundation.Private.Repositories;
using ImageUploader.Foundation.Private.Repositories.DbModels;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ImageUploader.Foundation.Private.Services
{
    internal class ImageUploadService : IImageUploadService
    {
        private readonly IImageRepository imageRepository;
        private readonly IMapper mapper;
        public ImageUploadService(IImageRepository imageRepository, IMapper mapper)
        {
            this.imageRepository = imageRepository ?? throw new ArgumentNullException(nameof(imageRepository));
            this.mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        public void DeleteImage(Guid id)
        {
            imageRepository.DeleteImage(id);
        }

        public ImageDto Get(Guid id)
        {
            var existingImage = imageRepository.Get(id);
            if (existingImage == null) { return null; }
            return mapper.Map<ImageDto>(existingImage);
        }

        public IEnumerable<ImageDto> GetAll()
        {
            return mapper.Map<IEnumerable<ImageDto>>(imageRepository.GetAll());
        }

        public IEnumerable<string> GetAllTags()
        {
            var allImages = imageRepository.GetAll();
            return allImages.SelectMany(x => x.Tags).Distinct();
        }
        public IEnumerable<ImageDto> GetByTags(IEnumerable<string> tags)
        {
            var imgs = imageRepository.GetByTags(tags);
            if (imgs == null || !imgs.Any()) return new List<ImageDto>();
            return mapper.Map<IEnumerable<ImageDto>>(imgs);
        }

        public IEnumerable<ImageDto> GetByTitle(string title)
        {
            return mapper.Map<IEnumerable<ImageDto>>(imageRepository.GetByTitle(title));
        }

        public ImageDto SetImageMetadata(Guid id, ImageDto imageDto)
        {
            imageDto.Id = id;
            var existingImage = imageRepository.Get(id);
            if (existingImage == null) { return null; }
            mapper.Map(imageDto, existingImage);
            return mapper.Map<ImageDto>(existingImage);
        }

        public Guid UploadImage(string fileName, string contentType, byte[] imgData)
        {
            var image = new DbImage()
            {
                FileName = fileName,
                Id = Guid.NewGuid(),
                ImageData = imgData,
                ContentType = contentType
            };
            imageRepository.CreateImage(image);
            return image.Id;
        }
    }
}
