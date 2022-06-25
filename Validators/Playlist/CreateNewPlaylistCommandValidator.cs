using DS.Application.CQRS.MediaLibrary.Commands;
using DS.Application.DTO.Playlist;
using FluentValidation;

namespace Validators.Playlist;

public class CreateNewPlaylistCommandValidator : AbstractValidator<CreateNewPlaylist.CreateNewPlaylistCommand>
{
    private static readonly string[] PictureFormats = { ".jpg", ".png", ".jpeg", ".heic" };
    private const int MinNameLength = 5;
    private const int MaxNameLength = 20;
    private const int MaxDescriptionLength = 500;
    
    
    public CreateNewPlaylistCommandValidator()
    {
        RuleFor(e => e.PlaylistCreationInfo)
            .Must(CheckDto);
    }

    private bool CheckDto(PlaylistCreationInfoDto playlistCreationInfoDto)
    {
        var cover = playlistCreationInfoDto.Cover;
        
        if (cover is not null && string.IsNullOrWhiteSpace(cover.FileName))
            if (!PictureFormats.Contains(cover.FileName[cover.FileName.LastIndexOf('.')..].ToLower()))
                return false;

        if (playlistCreationInfoDto.Name.Length <= MinNameLength && playlistCreationInfoDto.Name.Length >= MaxNameLength)
            return false;

        if (playlistCreationInfoDto.Description is not null &&
            playlistCreationInfoDto.Description.Length >= MaxDescriptionLength)
            return false;
        
        return true;
    }
}