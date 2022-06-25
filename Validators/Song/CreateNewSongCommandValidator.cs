using DS.Application.CQRS.MediaLibrary.Commands;
using DS.Application.DTO.Song;
using FluentValidation;

namespace Validators.Song;

public class CreateNewSongCommandValidator : AbstractValidator<CreateNewSong.CreateNewSongCommand>
{
    private static readonly string[] PictureFormats = { ".jpg", ".png", ".jpeg", ".heic" };
    private static readonly string[] SongFormats = { ".wav", ".mp3", ".aac", ".ogg", ".flac", ".aiff" };
    private const int MinNameLength = 5;
    private const int MaxNameLength = 20;

    CreateNewSongCommandValidator()
    {
        RuleFor(e => e.SongCreationInfo)
            .Must(CheckDto);
    }

    private bool CheckDto(SongCreationInfoDto songCreationInfoDto)
    {
        var cover = songCreationInfoDto.Cover;
        var song = songCreationInfoDto.Song;
        
        if (cover is not null && string.IsNullOrWhiteSpace(cover.FileName))
            if (!PictureFormats.Contains(cover.FileName[cover.FileName.LastIndexOf('.')..].ToLower()))
                return false;

        if (songCreationInfoDto.Name.Length <= MinNameLength && songCreationInfoDto.Name.Length >= MaxNameLength)
            return false;
        
        if (!SongFormats.Contains(song.FileName[song.FileName.LastIndexOf('.')..].ToLower()))
            return false;
        
        return true;
    }
}