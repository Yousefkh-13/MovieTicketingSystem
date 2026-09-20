namespace MovieTicketingSystem.Helpers
{
    public enum FileType
    {
        Img
    }
    public class FileUpload : IFileUpload
    {


        public string GenerateFileName(string fileName)
        {
            return $"{Guid.NewGuid().ToString()}-{DateTime.Now.ToString("dd-MM-yy")}{Path.GetExtension(fileName)}";
        }

        public string? GenerateFullPath(FileType fileType, string entityTypeFileName, string fileName)
        {
            switch (fileType)
            {
                case FileType.Img:
                    {
                        string filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "imgs", entityTypeFileName, fileName);
                        return filePath;
                    }
            }
            return null;
        }

        public bool UploadFileLocally(IFormFile file)
        {
            throw new NotImplementedException();
        }

        public bool UploadFileLocally(string Path, IFormFile file)
        {
            using (var Stream = System.IO.File.Create(Path))
            {
                file.CopyTo(Stream);
                
            }
            return true;
        }
        public bool DeleteFileLocally(string Path)
        {
            if (System.IO.File.Exists(Path))
            {
                System.IO.File.Delete(Path);
                return true;
            }
            return false;
        }
    }
}
