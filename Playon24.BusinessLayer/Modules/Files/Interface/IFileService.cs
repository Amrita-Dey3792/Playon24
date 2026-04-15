using Playon24.BusinessLayer.Modules.Files.Helpers;

namespace Playon24.BusinessLayer.Modules.Files.Interface
{
    public interface IFileService
    {
        Task<string> UploadAsync(FileUploadRequest? file, string folderName);
        Task DeleteUploadedFileAsync(string? publicUrl);
    }
}
