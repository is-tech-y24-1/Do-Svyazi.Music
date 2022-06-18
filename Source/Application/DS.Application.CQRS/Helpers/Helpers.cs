using Microsoft.AspNetCore.Http;

namespace DS.Application.CQRS.Helpers;

public static class Helpers
{
    public static bool ShouldGenerateUri(IFormFile? file)
        => file is not null && file.Length != 0;
}