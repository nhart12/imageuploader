using AutoMapper;
using FluentAssertions;
using ImageUploader.Core;
using ImageUploader.Foundation.Private.Repositories;
using ImageUploader.Foundation.Private.Repositories.DbModels;
using ImageUploader.Foundation.Private.Services;
using ImageUploader.Foundation.Private.Services.Mapping;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;

namespace ImageUploader.Internal.Tests
{
    [TestClass]
    public class ImageUploadServiceTests
    {
        private IImageUploadService imageUploadService;
        private Mock<IImageRepository> mockImageRepository;
        
        [TestInitialize]
        public void Setup()
        {
            mockImageRepository = new Mock<IImageRepository>();
            var realMapper = new MapperConfiguration(cfg => cfg.AddProfile<ImageMappingProfile>()).CreateMapper();
            imageUploadService = new ImageUploadService(mockImageRepository.Object, realMapper);
        }

        [TestMethod]
        public void GetImageById_ImageNotFound_ReturnsNull()
        {
            // Arrange
            var randomId = Guid.NewGuid();
            mockImageRepository.Setup(x => x.Get(randomId)).Returns((DbImage)null);
            // Act
            var result = imageUploadService.Get(randomId);
            // Assert
            result.Should().BeNull();
        }

        [TestMethod]
        public void SetImageMetadata_ImageNotFound_ReturnsNull()
        {
            // Arrange
            var randomId = Guid.NewGuid();
            mockImageRepository.Setup(x => x.Get(randomId)).Returns((DbImage)null);
            // Act
            var result = imageUploadService.SetImageMetadata(randomId, new Core.Models.ImageDto()
            {
                Description = "test",
                Tags = new string[] { "tag1", "tag2"},
                Title = "test"
            });
            // Assert
            result.Should().BeNull();
        }

        [TestMethod]
        public void SetImageMetadata_ExistingImage_ReturnsFullImageDtoWithMetadataSet()
        {
            // Arrange
            var randomId = Guid.NewGuid();
            var dbImage = new DbImage()
            {
                ContentType = "image/png",
                Id = randomId,
                FileName = "somefile.png"
            };
            mockImageRepository.Setup(x => x.Get(randomId)).Returns(dbImage);
            var metadata = new Core.Models.ImageDto()
            {
                Description = "test",
                Tags = new string[] { "tag1", "tag2" },
                Title = "test",
                FileName = "someotherfile.png"
            };

            // Act

            var result = imageUploadService.SetImageMetadata(randomId, metadata);

            // Assert

            //verify we didn't overwrite the original file name
            result.FileName.Should().Be(dbImage.FileName);
            //verify we set the metadata
            result.Description.Should().Be(metadata.Description);
            result.Title.Should().Be(metadata.Title);
            result.Tags.Should().Contain("tag1").And.Contain("tag2");
        }
    }
}
