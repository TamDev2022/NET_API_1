using Azure.Storage.Blobs.Models;
using Azure.Storage.Blobs;
using Azure;
using NET_API_1.Interfaces.IServices;
using NET_API_1.Models.DTO;
using System.Reflection.Metadata;
using static System.Reflection.Metadata.BlobBuilder;
using static NET_API_1.Configurations.AppSettings;

namespace NET_API_1.Services
{
    public class AzureStorageService : IAzureStorageService
    {
        #region Dependency Injection / Constructor

        private readonly BlobAzureSettings _blobAzureSettings;
        private readonly ILogger<AzureStorageService> _logger;
        private readonly IFileService _fileService;

        public AzureStorageService(IConfiguration configuration, ILogger<AzureStorageService> logger,
            IFileService fileService, BlobAzureSettings blobAzureSettings)
        {
            _logger = logger;
            _fileService = fileService;
            _blobAzureSettings = blobAzureSettings;
        }
        #endregion
        public Task<BlobResponseDto?> DeleteAsync(string blobFilename)
        {
            throw new NotImplementedException();
        }

        public async Task<BlobDto?> DownloadAsync(string blobFilename)
        {
            // Get a reference to a container named in appsettings.json
            BlobContainerClient client =
                new BlobContainerClient(_blobAzureSettings.BlobConnectionString, _blobAzureSettings.BlobContainerName);

            try
            {
                // Get a reference to the blob uploaded earlier from the API in the container from configuration settings
                BlobClient file = client.GetBlobClient(blobFilename);

                // Check if the file exists in the container
                if (await file.ExistsAsync())
                {
                    var data = await file.OpenReadAsync();
                    Stream blobContent = data;

                    // Download the file details async
                    var content = await file.DownloadContentAsync();

                    // Add data to variables in order to return a BlobDto
                    string name = blobFilename;
                    string contentType = content.Value.Details.ContentType;

                    // Create new BlobDto with blob data from variables
                    return new BlobDto { Content = blobContent, Name = name, ContentType = contentType };
                }
            }
            catch (RequestFailedException ex)
                when (ex.ErrorCode == BlobErrorCode.BlobNotFound)
            {
                // Log error to console
                _logger.LogError($"File {blobFilename} was not found.");
            }

            // File does not exist, return null and handle that in requesting method
            return null;
        }

        public async Task<List<BlobDto>> ListAsync()
        {
            // Get a reference to a container named in appsettings.json
            BlobContainerClient container =
                new BlobContainerClient(_blobAzureSettings.BlobConnectionString, _blobAzureSettings.BlobContainerName);

            // Create a new list object for 
            List<BlobDto> files = new List<BlobDto>();

            await foreach (BlobItem file in container.GetBlobsAsync())
            {
                // Add each file retrieved from the storage container to the files list by creating a BlobDto object
                string uri = container.Uri.ToString();
                var name = file.Name;
                var fullUri = $"{uri}/{name}";

                files.Add(new BlobDto
                {
                    Uri = fullUri,
                    Name = name,
                    ContentType = file.Properties.ContentType
                });
            }

            // Return all files to the requesting method
            return files;
        }

        public async Task<BlobResponseDto?> UploadAsync(IFormFile file)
        {
            // Create new upload response object that we can return to the requesting method
            BlobResponseDto response = new();

            // Get a reference to a container named in appsettings.json and then create it
            BlobContainerClient container = new BlobContainerClient(_blobAzureSettings.BlobConnectionString, _blobAzureSettings.BlobContainerName);
            try
            {
                var NewFileName = _fileService.GenarateFileNameWebp(file.FileName);

                // Get a reference to the blob just uploaded from the API in a container from configuration settings
                BlobClient client = container.GetBlobClient(NewFileName);

                // Open a stream for the file we want to upload
                await using (Stream? data = file.OpenReadStream())
                {
                    // Upload the file async
                    await client.UploadAsync(data);
                }

                // Everything is OK and file got uploaded
                response.Status = $"File {NewFileName} Uploaded Successfully";
                response.Error = false;
                response.Blob.Uri = client.Uri.AbsoluteUri;
                response.Blob.Name = client.Name;
            }
            catch (RequestFailedException ex) when (ex.ErrorCode == BlobErrorCode.BlobAlreadyExists)
            {
                _logger.LogError($"File with name {file.FileName} already exists in container. Set another name to store the file in the container: '{_blobAzureSettings.BlobContainerName}.'");
                response.Status = $"File with name {file.FileName} already exists. Please use another name to store your file.";
                response.Error = true;
                return response;
            }
            // If we get an unexpected error, we catch it here and return the error message
            catch (RequestFailedException ex)
            {
                // Log error to console and create a new response we can return to the requesting method
                _logger.LogError($"Unhandled Exception. ID: {ex.StackTrace} - Message: {ex.Message}");
                response.Status = $"Unexpected error: {ex.StackTrace}. Check log with StackTrace ID.";
                response.Error = true;
                return response;
            }
            return response;
        }


    }
}
