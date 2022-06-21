using System.IO;
using AutoMapper;
using DS.Application.DTO.ListeningQueue;
using DS.Application.DTO.MusicUser;
using DS.Application.DTO.Playlist;
using DS.Application.DTO.Song;
using DS.Tests.Stubs;
using System.Linq;
using System.Text;
using Microsoft.AspNetCore.Mvc;

namespace DS.Tests;

public class DomainToResponseTests : Profile
{
    public DomainToResponseTests()
    {
        CreateMap<Domain.ListeningQueue, ListeningQueueInfoDto>();

        CreateMap<Domain.Song, SongInfoDto>()
            .ForMember(dest => dest.AuthorName, opt =>
                opt.MapFrom(src => src.Author.Name))
            .ForMember(dest => dest.GenreName, opt =>
                opt.MapFrom(src => src.Genre.Name))
            .ForMember(dest => dest.Content, opt => 
                opt.MapFrom(src => FileStub.GetResultFileDummy()))
            .ForMember(dest => dest.Cover, opt =>
                opt.MapFrom(dest => FileStub.GetResultFileDummy()));

        CreateMap<Domain.MusicUser, MusicUserInfoDto>()
            .ForMember(dest => dest.ProfilePicture, opt =>
                opt.MapFrom(src => FileStub.GetResultFileDummy()));

            CreateMap<Domain.Playlist, PlaylistInfoDto>()
            .ForMember(dest => dest.Cover, opt =>
                opt.MapFrom(src => FileStub.GetResultFileDummy()));
    }
}
