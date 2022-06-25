using DS.Application.CQRS.MusicUser.Commands;
using DS.Application.DTO.MusicUser;
using FluentValidation;

namespace Validators.MusicUser;

public class AddMusicUserCommandValidator : AbstractValidator<AddMusicUser.AddUserCommand>
{
    private static readonly string[] PictureFormats = { ".jpg", ".png", ".jpeg", ".heic" };
    private const int MinNameLength = 5;
    private const int MaxNameLength = 20;
    
    public AddMusicUserCommandValidator()
    {
        RuleFor(e => e.MusicUserCreationInfo)
            .Must(CheckDto);
    }

    private bool CheckDto(MusicUserCreationInfoDto musicUserCreationInfoDto)
    {
        var file = musicUserCreationInfoDto.ProfilePicture;
        
        if (file is not null && string.IsNullOrWhiteSpace(file.FileName))
            if (!PictureFormats.Contains(file.FileName[file.FileName.LastIndexOf('.')..].ToLower()))
                return false;

        if (musicUserCreationInfoDto.Name.Length <= MinNameLength && musicUserCreationInfoDto.Name.Length >= MaxNameLength)
            return false;
        
        return true;
    }
}