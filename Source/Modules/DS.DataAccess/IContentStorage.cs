namespace DS.DataAccess;

public interface IContentStorage
{
    string GenerateUri();
    Task CreateStorageFile(string uri, byte[] data);
    void DeleteStorageFile(string uri);
    Task<byte[]> GetFileData(string uri);
}