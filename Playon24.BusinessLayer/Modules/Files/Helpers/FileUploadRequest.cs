namespace Playon24.BusinessLayer.Modules.Files.Helpers
{
    public sealed class FileUploadRequest : IDisposable
    {
        public string FileName { get; }
        public long Length { get; }
        public Stream Content { get; }
        public FileUploadRequest(string fileName, long length, Stream content)
        {
            FileName = fileName;
            Length = length;
            Content = content;
        }

        public void Dispose()
        {
            Content.Dispose();
        }
    }
}
