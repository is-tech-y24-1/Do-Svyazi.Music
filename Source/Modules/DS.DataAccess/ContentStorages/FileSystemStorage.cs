using DS.Common.Exceptions;
using DS.Common.Extensions;

namespace DS.DataAccess.ContentStorages;

public class FileSystemStorage : IContentStorage
{
    private readonly string _storageDirectoryPath;

    public FileSystemStorage(string storageDirectoryPath)
    {
        _storageDirectoryPath = storageDirectoryPath.ThrowIfNull();
        Directory.CreateDirectory(storageDirectoryPath);
    }

    public string GenerateUri()
    {
        return Guid.NewGuid().ToString();
    }

    public async Task CreateStorageFile(string uri, byte[] data)
    {
        uri.ThrowIfNull(uri);
        string pathToFile = Path.Combine(_storageDirectoryPath, uri);
        await File.WriteAllBytesAsync(pathToFile, data);
    }

    public void DeleteStorageFile(string uri)
    {
        CheckIfContentExists(uri);
        var file = new FileInfo(Path.Combine(_storageDirectoryPath, uri));
        file.Delete();
    }

    public async Task<byte[]> GetFileData(string uri)
    {
        CheckIfContentExists(uri);
        return await File.ReadAllBytesAsync(Path.Combine(_storageDirectoryPath, uri));
    }

    private void CheckIfContentExists(string uri)
    {
        uri.ThrowIfNull();
        if (!File.Exists(Path.Combine(_storageDirectoryPath, uri)))
            throw new ContentNotFoundException($"File with URI: {uri} not found in storage");
    }
}