using AutoMapper;
using DS.Application.DTO.ListeningQueue;
using DS.Application.DTO.MusicUser;
using DS.Application.DTO.Playlist;
using DS.Application.DTO.Song;
using DS.DataAccess;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.StaticFiles;

namespace DS.Application.CQRS.Mapping;

public class DomainToResponse : Profile
{
    private readonly IContentStorage _storage;
    public DomainToResponse(IContentStorage storage)
    {
        _storage = storage;
        CreateMap<Domain.ListeningQueue, ListeningQueueInfoDto>();

        CreateMap<Domain.Song, SongInfoDto>()
            .ForMember(dest => dest.AuthorName, opt =>
                opt.MapFrom(src => src.Author.Name))
            .ForMember(dest => dest.GenreName, opt =>
                opt.MapFrom(src => src.Genre.Name))
            .ForMember(src => src.Cover, opt =>
                opt.MapFrom( dest => GetFileData(dest.CoverUri, dest.CoverUri)))
            .ForMember(src => src.Content, opt =>
                opt.MapFrom(dest => GetFileData(dest.ContentUri, dest.Name)));

        CreateMap<Domain.MusicUser, MusicUserInfoDto>()
            .ForMember(src => src.ProfilePicture, opt =>
                opt.MapFrom(dest => GetFileData(dest.ProfilePictureUri, dest.ProfilePictureUri)));
        
        CreateMap<Domain.Playlist, PlaylistInfoDto>()
            .ForMember(src => src.Cover, opt =>
                opt.MapFrom(dest => GetFileData(dest.CoverUri, dest.CoverUri)));
    }

    private async Task<FileResult?> GetFileData(string? uri, string? fileName)
    {
        if (uri is null)
            return null;

        var fileData = await _storage.GetFileData(uri);
        using var stream = new MemoryStream(fileData.Content);
        var fileProvider = new FileExtensionContentTypeProvider();
        
        if (!fileProvider.TryGetContentType(fileData.FileNameWithExtension, out string contentType))
            throw new ArgumentOutOfRangeException($"Unable to find Content Type for file name {fileData.FileNameWithExtension}.");
        
        return new FileStreamResult(stream, contentType);
    }
}