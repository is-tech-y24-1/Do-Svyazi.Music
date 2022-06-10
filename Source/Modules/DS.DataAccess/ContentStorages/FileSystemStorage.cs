namespace DS.DataAccess.ContentStorages;

public class FileSystemStorage : IContentStorage
{
    public string GenerateUri()
    {
        return Guid.NewGuid().ToString();
    }

    public void CreateStorageFile(string uri, string fileName, byte[] data)
    {
        throw new NotImplementedException();
    }

    public void DeleteStorageFile(string uri)
    {
        throw new NotImplementedException();
    }

    public byte[] GetFileData(string uri)
    {
        throw new NotImplementedException();
    }
}