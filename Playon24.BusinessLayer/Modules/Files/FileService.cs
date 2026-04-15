using Playon24.BusinessLayer.Exceptions;
using Playon24.BusinessLayer.Modules.Files.Helpers;
using Playon24.BusinessLayer.Modules.Files.Interface;

namespace Playon24.BusinessLayer.Modules.Files
{
    public class FileService : IFileService
    {
        private readonly string _webRootPath;

        public FileService(string webRootPath)
        {
            _webRootPath = webRootPath;
        }

        public async Task<string> UploadAsync(FileUploadRequest? file, string folderName)
        {
            if (file == null || file.Length == 0)
                throw new EmptyFileException("File is empty");

            var allowedExtensions = new[] { ".jpg", ".jpeg", ".png", ".webp" };
            var extension = Path.GetExtension(file.FileName).ToLowerInvariant();

            if (!allowedExtensions.Contains(extension))
                throw new FileTypeException("Invalid file type");

            if (file.Length > 5 * 1024 * 1024)
                throw new FileSizeExceedException("File size exceeds 5MB");

            var fileName = Guid.NewGuid().ToString() + extension;

            var folderPath = Path.Combine(_webRootPath, "uploads", folderName); // wwwroot/uploads/folderName

            if (!Directory.Exists(folderPath))
                Directory.CreateDirectory(folderPath);

            var filePath = Path.Combine(folderPath, fileName); // wwwroot/uploads/folderName/uniqueFileName.jpg

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await file.Content.CopyToAsync(stream);
            }

            return $"/uploads/{folderName}/{fileName}";
        }

        public Task DeleteUploadedFileAsync(string? publicUrl)
        {
            if (string.IsNullOrWhiteSpace(publicUrl))
                return Task.CompletedTask;

            var normalized = publicUrl.Trim().Replace('\\', '/');
            if (!normalized.StartsWith("/uploads/", StringComparison.OrdinalIgnoreCase))
                return Task.CompletedTask;

            var relative = normalized.TrimStart('/').Replace('/', Path.DirectorySeparatorChar);
            if (relative.Split(Path.DirectorySeparatorChar).Any(s => s is "." or ".."))
                return Task.CompletedTask;

            var fullPath = Path.GetFullPath(Path.Combine(_webRootPath, relative));
            var uploadsRoot = Path.GetFullPath(Path.Combine(_webRootPath, "uploads"));

            var relativeToUploads = Path.GetRelativePath(uploadsRoot, fullPath);
            if (relativeToUploads.StartsWith("..", StringComparison.Ordinal) || Path.IsPathRooted(relativeToUploads))
                return Task.CompletedTask;

            if (File.Exists(fullPath))
                File.Delete(fullPath);

            return Task.CompletedTask;
        }
    }
}