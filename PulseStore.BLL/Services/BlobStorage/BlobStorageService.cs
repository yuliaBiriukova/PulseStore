using Azure;
using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using PulseStore.BLL.Models.BlobStorage;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace PulseStore.BLL.Services.BlobStorage
{
    public class BlobStorageService : IBlobStorageService
    {
        private readonly BlobServiceClient _blobServiceClient;
        private readonly ILogger<BlobStorageService> _logger;

        public string ContainerName { get; set; }

        public BlobStorageService(ILogger<BlobStorageService> logger, BlobServiceClient blobService, IConfiguration config)
        {
            _blobServiceClient = blobService;
            _logger = logger;
            ContainerName = config["BlobStorageContainers:PhotoContainerName"];
        }

        public async Task CheckContainer()
        {
            try
            {
                var blobContainer = _blobServiceClient.GetBlobContainers().FirstOrDefault(p => p.Name == ContainerName);
                if (blobContainer is null)
                {
                    BlobContainerClient container = await _blobServiceClient.CreateBlobContainerAsync(ContainerName);
                    if (await container.ExistsAsync())
                    {
                        _logger.LogInformation($"Container {ContainerName} is created!");
                    }
                }
                else
                {
                    _logger.LogInformation($"Container {ContainerName} exists");
                }
                    
            }
            catch (RequestFailedException e)
            {
                _logger.LogError($"Container is not created! Error: {e.Message}");
            }
        }

        public async Task<BlobResponseDto> DeleteAsync(string blobFilename)
        {
            var client = _blobServiceClient.GetBlobContainerClient(ContainerName);

            BlobClient file = client.GetBlobClient(blobFilename);

            try
            {
                // Delete the file
                await file.DeleteAsync();
            }
            catch (RequestFailedException ex)
                when (ex.ErrorCode == BlobErrorCode.BlobNotFound)
            {
                // File did not exist, log to console and return new response to requesting method
                _logger.LogError($"File {blobFilename} was not found.");
                return new BlobResponseDto { Error = true, Status = $"File with name {blobFilename} not found." };
            }

            // Return a new BlobResponseDto to the requesting method
            return new BlobResponseDto { Error = false, Status = $"File: {blobFilename} has been successfully deleted." };
        }

        public async Task<BlobDto> DownloadAsync(string blobFilename)
        {
            // Get a reference to a container named in appsettings.json
            BlobContainerClient client = _blobServiceClient.GetBlobContainerClient(ContainerName);

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

        public async Task<BlobResponseDto> UploadAsync(IFormFile file, bool addFileExtension = false)
        {
            BlobResponseDto response = new();
            Random random = new Random();
            // Get a reference to a container named in appsettings.json and then create it
            BlobContainerClient container = _blobServiceClient.GetBlobContainerClient(ContainerName);

            string fileName = Guid.NewGuid().ToString();

            if(addFileExtension)
            {
                fileName += Path.GetExtension(file.FileName);
            }

            try
            {
                // Get a reference to the blob just uploaded from the API in a container from configuration settings
                BlobClient client = container.GetBlobClient(fileName);

                // Open a stream for the file we want to upload
                await using (Stream? data = file.OpenReadStream())
                {
                    // Upload the file async
                    await client.UploadAsync(data);
                }

                // Everything is OK and file got uploaded
                response.Status = $"File {fileName} Uploaded Successfully";
                response.Error = false;
                response.Blob.Uri = client.Uri.AbsoluteUri;
                response.Blob.Name = client.Name;

            }
            // If the file already exists, we catch the exception and do not upload it
            catch (RequestFailedException ex)
               when (ex.ErrorCode == BlobErrorCode.BlobAlreadyExists)
            {
                _logger.LogError($"File with name {fileName} already exists in container. Set another name to store the file in the container: '{ContainerName}.'");
                response.Status = $"File with name {fileName} already exists. Please use another name to store your file.";
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

            // Return the BlobUploadResponse object
            return response;
        }

        public async Task<BlobResponseDto> UploadAsync(BlobDto file)
        {
            BlobResponseDto response = new();
            Random random = new Random();
            // Get a reference to a container named in appsettings.json and then create it
            BlobContainerClient container = _blobServiceClient.GetBlobContainerClient(ContainerName);
            string fileName = Guid.NewGuid().ToString();

            try
            {
                // Get a reference to the blob just uploaded from the API in a container from configuration settings
                BlobClient client = container.GetBlobClient(fileName);

                // Open a stream for the file we want to upload
                await using (Stream? data = file.Content)
                {
                    // Upload the file async
                    await client.UploadAsync(data);
                }

                // Everything is OK and file got uploaded
                response.Status = $"File {fileName} Uploaded Successfully";
                response.Error = false;
                response.Blob.Uri = client.Uri.AbsoluteUri;
                response.Blob.Name = client.Name;

            }
            // If the file already exists, we catch the exception and do not upload it
            catch (RequestFailedException ex)
               when (ex.ErrorCode == BlobErrorCode.BlobAlreadyExists)
            {
                _logger.LogError($"File with name {fileName} already exists in container. Set another name to store the file in the container: '{ContainerName}.'");
                response.Status = $"File with name {fileName} already exists. Please use another name to store your file.";
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

            // Return the BlobUploadResponse object
            return response;
        }
    }
}
