namespace MovieTicketingSystem.Helpers
{
    public interface IFileUpload
    {
        string GenerateFileName(string fileName);
        string? GenerateFullPath(FileType fileType,string entityTypeFileName, string fileName);
        bool UploadFileLocally(string Path, IFormFile file);
        bool DeleteFileLocally(string Path);
    }
}
