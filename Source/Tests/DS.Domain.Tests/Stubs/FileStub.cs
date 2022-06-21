using System.IO;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Primitives;
using Microsoft.Net.Http.Headers;

namespace DS.Tests.Stubs;

public static class FileStub
{
   private static FileStreamResult _fileStreamResult = new FileStreamResult
   (
      new MemoryStream(Encoding.UTF8.GetBytes("This is a dummy file")),
      "text/plain"
   );
   
   public static IFormFile GetDummyFile()
   { 
      byte[] bytes = Encoding.UTF8.GetBytes("This is a dummy file");
      return new FormFile
      (
         new MemoryStream(bytes), 
         0, 
         bytes.Length, 
         "Data", 
         "dummy.txt"
      );
   }

   public static FileStreamResult GetResultFileDummy()
      => _fileStreamResult;
}