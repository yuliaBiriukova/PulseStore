using PulseStore.BLL.Entities;
using PulseStore.BLL.Models.BlobStorage;
using PulseStore.BLL.Repositories;
using PulseStore.BLL.Services.BlobStorage;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PulseStore.BLL.Services.Photo
{
    public class PhotoService : IPhotoService
    {
        private readonly IBlobStorageService _blobStorageService;
        private readonly IPhotoRepository _photoRepository;

        public PhotoService(IBlobStorageService blobStorageService, IPhotoRepository photoRepository)
        {
            _blobStorageService = blobStorageService;
            _photoRepository = photoRepository;
        }

        public async Task<string> DeletePhotoesAsync(int[] ids)
        {
            string result = "";
            foreach (int id in ids)
            {
                var photoName = await _photoRepository.GetNameFromUrl(id);
                if (!string.IsNullOrEmpty(photoName))
                {
                    var blobResult = await _blobStorageService.DeleteAsync(photoName);
                    var dbResult = await _photoRepository.DeletePhoto(id);
                    if (blobResult.Error == false && dbResult)
                    {
                       result+= $"Photo with id {id} was deleted!\n";
                    }
                    else { result += "Error\n"; }
                }
                else
                {
                    result += $"Ther was no such photo {id}!\n";
                }
                
            }
            return result;

            
        }

        public async Task<List<int>> UploadAsync(IFormFile[] files, int productId)
        {
            List<int> result = new List<int>();
            foreach (var file in files)
            {
                var blobResponse = await _blobStorageService.UploadAsync(file);
                var sequence = await _photoRepository.CheckSequence(productId);
                ProductPhoto photo = new ProductPhoto()
                {
                    ProductId = productId,
                    SequenceNumber = sequence,
                    ImagePath = blobResponse.Blob.Uri
                };
                result.Add(await _photoRepository.AddPhoto(photo));
            }


            return result;
        }

        public async Task<List<int>> UploadAsync(string[] urls, int productId)
        {
            List<int> result = new List<int>();
            foreach (var url in urls)
            {
                int pos = url.LastIndexOf("/") + 1;
                var name = url.Substring(pos, url.Length - pos);
                var file = await _blobStorageService.DownloadAsync(name);
                var blobResponse = await _blobStorageService.UploadAsync(file);
                var sequence = await _photoRepository.CheckSequence(productId);
                ProductPhoto photo = new ProductPhoto()
                {
                    ProductId = productId,
                    SequenceNumber = sequence,
                    ImagePath = blobResponse.Blob.Uri
                };
                result.Add(await _photoRepository.AddPhoto(photo));
            }


            return result;
        }
    }
}
