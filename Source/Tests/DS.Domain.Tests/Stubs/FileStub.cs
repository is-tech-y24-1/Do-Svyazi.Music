using System.IO;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace DS.Tests.Stubs;

public static class FileStub
{
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
}