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

    public string GenerateUri(string fileName)
    {
        fileName.ThrowIfNull();
        return Path.Combine(Guid.NewGuid().ToString(), fileName);
    }

    public async Task CreateStorageFile(string uri, byte[] data)
    {
        uri.ThrowIfNull(uri);
        data.ThrowIfNull();
        string pathToFile = Path.Combine(_storageDirectoryPath, uri);
        await File.WriteAllBytesAsync(pathToFile, data);
    }

    public void DeleteStorageFile(string uri)
    {
        CheckIfContentExists(uri);
        var file = new FileInfo(Path.Combine(_storageDirectoryPath, uri));
        file.Delete();
    }

    public async Task<FileData> GetFileData(string uri)
    {
        CheckIfContentExists(uri);
        
        string filePath = Path.Combine(_storageDirectoryPath, uri);
        string fileNameWithExtension = Path.GetFileName(filePath);
        
        byte[] content = await File.ReadAllBytesAsync(filePath);

        return new FileData(content, fileNameWithExtension);
    }

    private void CheckIfContentExists(string uri)
    {
        uri.ThrowIfNull();
        if (!File.Exists(Path.Combine(_storageDirectoryPath, uri)))
            throw new ContentNotFoundException($"File with URI: {uri} not found in storage");
    }
}