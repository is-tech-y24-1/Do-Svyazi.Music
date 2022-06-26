using Microsoft.AspNetCore.Mvc;

namespace DS.Application.DTO.MusicUser;

public record MusicUserInfoDto(Guid Id, string Name)
{
    // Needed for mapper to work properly
    public MusicUserInfoDto() 
        : this(Guid.Empty, string.Empty) { }
};