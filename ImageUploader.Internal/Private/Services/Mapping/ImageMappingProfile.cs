using AutoMapper;
using ImageUploader.Core.Models;
using ImageUploader.Foundation.Private.Repositories.DbModels;

namespace ImageUploader.Foundation.Private.Services.Mapping
{
    class ImageMappingProfile : Profile
    {
        public ImageMappingProfile()
        {
            CreateMap<ImageDto, DbImage>()
                .ForMember(dest => dest.Description, opts => opts.MapFrom(src => src.Description))
                .ForMember(dest => dest.Title, opts => opts.MapFrom(src => src.Title))
                .ForMember(dest => dest.Tags, opts => opts.MapFrom(src => src.Tags))
                .ForMember(dest => dest.Id, opts => opts.Ignore())
                .ForMember(dest => dest.FileName, opts => opts.Ignore())
                .ForMember(dest => dest.ImageData, opts => opts.Ignore())
                .ForMember(dest => dest.ContentType, opts => opts.Ignore())
                ;

            CreateMap<DbImage, ImageDto>()
                .ForMember(dest => dest.Description, opts => opts.MapFrom(src => src.Description))
                .ForMember(dest => dest.Title, opts => opts.MapFrom(src => src.Title))
                .ForMember(dest => dest.Tags, opts => opts.MapFrom(src => src.Tags))
                .ForMember(dest => dest.Id, opts => opts.MapFrom(src => src.Id))
                .ForMember(dest => dest.FileName, opts => opts.MapFrom(src => src.FileName))
                .ForMember(dest => dest.Path, opts => opts.MapFrom(src => $"{src.Id}/{src.FileName}"))
                .ForMember(dest => dest.ImageData, opts => opts.MapFrom(src => src.ImageData))
                .ForMember(dest => dest.ContentType, opts => opts.MapFrom(src => src.ContentType))
                ;
        }
    }
}
