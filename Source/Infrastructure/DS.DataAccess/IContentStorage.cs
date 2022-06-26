namespace DS.DataAccess;

public record FileData(byte[] Content, string FileNameWithExtension);
public interface IContentStorage
{
    string GenerateUri(string fileName);
    Task CreateStorageFile(string uri, byte[] data);
    void DeleteStorageFile(string uri);
    Task<FileData> GetFileData(string uri);
}