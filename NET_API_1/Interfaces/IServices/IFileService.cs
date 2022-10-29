namespace NET_API_1.Interfaces.IServices
{
    public interface IFileService
    {
        public Tuple<bool, string?> IsImageFile(IFormFile file);
        public string GenarateFileNameWebp(string fileName);
        public Task<byte[]> GetFileAsync(string fileName);
        public Task UploadFileAsync(Stream file, string fileName);
        public Task<string> UploadFileAsync(IFormFile file);


    }
}
