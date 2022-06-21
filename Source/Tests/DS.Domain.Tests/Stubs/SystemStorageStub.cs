using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using DS.DataAccess;

namespace DS.Tests.Stubs;

public class SystemStorageStub : IContentStorage
{
    private readonly List<FileInfo> _files = new();
    public string GenerateUri(string fileName) => Guid.NewGuid().ToString();

    public Task CreateStorageFile(string uri, byte[] data)
    {
        var fileInfo = new FileInfo(uri);
        _files.Add(fileInfo);
        
        return Task.CompletedTask;
    }

    public void DeleteStorageFile(string uri)
        => _files.Remove(_files.Find(fi => fi.Name == uri)!);
    

    public Task<FileData> GetFileData(string uri)
    {
        var fileToFind = _files.Find(fi => fi.Name == uri)!;
        byte[] bytes = Encoding.ASCII.GetBytes(fileToFind.Name);
        
        var fileData = new FileData(bytes, "dummy");
        return Task.FromResult(fileData);
    }
}