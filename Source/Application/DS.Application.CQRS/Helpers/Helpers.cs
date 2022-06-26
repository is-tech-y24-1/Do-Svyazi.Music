using DS.Common;
using DS.DataAccess;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.StaticFiles;

namespace DS.Application.CQRS.Helpers;

public static class Helpers
{
    public static bool ShouldGenerateUri(IFormFile? file)
        => file is not null && file.Length != 0;
    public static async Task<FileStreamResult?> GetFileData(string? uri, IContentStorage storage)
    {
        if (uri is null)
            return null;

        var fileData = await storage.GetFileData(uri);
        var stream = new MemoryStream(fileData.Content);
        var fileProvider = new FileExtensionContentTypeProvider();
        
        if (!fileProvider.TryGetContentType(fileData.FileNameWithExtension, out string contentType))
            throw new ArgumentOutOfRangeException($"Unable to find Content Type for file name {fileData.FileNameWithExtension}.");
        
        return new FileStreamResult(stream, contentType);
    }

    public static async Task UploadFile
    (
        Stream stream,
        string? uri,
        IContentStorage storage
    )
    {
        byte[] buffer = new byte[Constants.BufferSize];
        using (MemoryStream ms = new MemoryStream())
        {
            int read;
            while ((read = await stream.ReadAsync(buffer, 0, buffer.Length)) > 0)
            {
                ms.Write(buffer, 0, read);
            }
                
            if (uri is not null)
                await storage.CreateStorageFile(uri, ms.ToArray());
        }
    }
}