using PulseStore.BLL.Models.BlobStorage;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PulseStore.BLL.Services.Photo
{
    public interface IPhotoService
    {
        /// <summary>
        /// Get <see cref="List{T}"/> of <see cref="int"/> with photos id which was Uploaded to <see cref="BlobStorage"/> and database
        /// </summary>
        /// <param name="files"> Array of files which should be uploaded.</param>
        /// <param name="productId"> Id of product which will contain the photoes</param>
        /// <returns><see cref="List{T}"/> of <see cref="int"/> with photoes id.</returns>
        Task<List<int>> UploadAsync(IFormFile[] files, int productId);
        Task<List<int>> UploadAsync(string[] files, int productId);
        /// <summary>
        /// Get <see cref="string"/> which show what photo was deleted.
        /// </summary>
        /// <param name="ids"> Array of <see cref="int"/> with photoes that should be deleted.</param>
        /// <returns><see cref="string"/> with ids of deleted photoes</returns>
        Task<string> DeletePhotoesAsync(int[] ids);
    }
}
