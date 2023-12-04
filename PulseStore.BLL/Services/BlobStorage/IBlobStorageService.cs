using PulseStore.BLL.Models.BlobStorage;
using Microsoft.AspNetCore.Http;

namespace PulseStore.BLL.Services.BlobStorage
{
    public interface IBlobStorageService
    {
        public string ContainerName { get; set; }

        /// <summary>
        ///     This method uploads a file submitted with the request
        /// </summary>
        /// <param name="file"><see cref="IFormFile"/>File for upload</param>
        /// <param name="addFileExtension">Optional bool flag. If true adds file extension to file name; by default is false.</param>
        /// <returns>Blob with status</returns>
        Task<BlobResponseDto> UploadAsync(IFormFile file, bool addFileExtension = false);

        Task<BlobResponseDto> UploadAsync(BlobDto file);

        /// <summary>
        /// This method downloads a file with the specified filename
        /// </summary>
        /// <param name="blobFilename">Filename</param>
        /// <returns>Blob</returns>
        Task<BlobDto> DownloadAsync(string blobFilename);

        /// <summary>
        /// This method deleted a file with the specified filename
        /// </summary>
        /// <param name="blobFilename">Filename</param>
        /// <returns><see cref="BlobResponseDto"/> with status</returns>
        Task<BlobResponseDto> DeleteAsync(string blobFilename);

        /// <summary>
        /// This method checks the existence of container, if it doesn't exist, it will create container.
        /// </summary>
        Task CheckContainer();
    }
}