using AutoMapper;
using DS.Application.DTO.ListeningQueue;
using DS.Application.DTO.MusicUser;
using DS.Application.DTO.Playlist;
using DS.Application.DTO.Song;

namespace DS.Application.CQRS.Mapping;

public class DomainToResponse : Profile
{
    public DomainToResponse()
    {
        CreateMap<Domain.ListeningQueue, ListeningQueueInfoDto>();

        CreateMap<Domain.Song, SongInfoDto>()
            .ForMember(dest => dest.AuthorName, opt =>
                opt.MapFrom(src => src.Author.Name))
            .ForMember(dest => dest.GenreName, opt =>
                opt.MapFrom(src => src.Genre.Name));
        
        CreateMap<Domain.MusicUser, MusicUserInfoDto>();
        
        CreateMap<Domain.Playlist, PlaylistInfoDto>();
    }
}