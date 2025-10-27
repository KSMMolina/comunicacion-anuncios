using AutoMapper;
using Communication.Announcements.Application.DTOs.Announcements;
using Communication.Announcements.Domain.Entities;

namespace Communication.Announcements.Application.Mapping;

public class ApplicationProfile : Profile
{
    public ApplicationProfile()
    {
        CreateMap<Announcement, AnnouncementResponse>()
            .ForMember(dest => dest.CreatedByName, opt => opt.MapFrom(src => src.CreatedByUser != null ? src.CreatedByUser.FullName : null))
            .ForMember(dest => dest.UpdatedByName, opt => opt.MapFrom(src => src.UpdatedByUser != null ? src.UpdatedByUser.FullName : null));
    }
}
