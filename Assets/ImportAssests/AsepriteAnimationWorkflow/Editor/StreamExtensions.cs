using System.IO;
using System.IO.Compression;

namespace APIShift.AsepriteAnimationWorkflow
{
  public static class StreamExtensions
  {
    public static Stream GzipDeflate(this Stream stream)
    {
      var deflatedStream = new MemoryStream();
      using (var gzip = new DeflateStream(stream, CompressionMode.Decompress))
      {
        var buffer = new byte[1024];
        var len = 1;
        while (len > 0)
        {
          len = gzip.Read(buffer, 0, buffer.Length);
          deflatedStream.Write(buffer, 0, len);
        }
      }

      deflatedStream.Position = 0;
      return deflatedStream;
    }
  }
}