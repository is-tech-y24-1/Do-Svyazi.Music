namespace DS.DataAccess;

public interface IContentStorage
{
    string GenerateUri();
    void CreateStorageFile(string uri, string fileName, byte[] data);
    void DeleteStorageFile(string uri);
    byte[] GetFileData(string uri);
}