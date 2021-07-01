using AutoMapper;
using ImageUploader.Core;
using ImageUploader.Foundation.Private.Repositories;
using ImageUploader.Foundation.Private.Services;
using ImageUploader.Foundation.Private.Services.Mapping;
using Microsoft.Extensions.DependencyInjection;

namespace ImageUploader.Internal.Public
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddInternalServices(this IServiceCollection services)
        {
            return services
                    .AddSingleton<IImageUploadService, ImageUploadService>()
                    .AddSingleton<IImageRepository, MockImageRepository>()
                    .AddSingleton<IMapper>(new Mapper(new MapperConfiguration(cfg => cfg.AddProfile<ImageMappingProfile>())))
                    ;
        }
    }
}
