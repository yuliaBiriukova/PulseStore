using PulseStore.BLL.Entities;
using PulseStore.BLL.Repositories;
using PulseStore.DAL.Database;
using LanguageExt.Common;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Threading.Tasks;

namespace PulseStore.DAL.Repositories
{
    public class PhotoRepository: IPhotoRepository
    {
        private readonly PulseStoreContext _dbContext;

        public PhotoRepository(PulseStoreContext dbContext)
        {
            _dbContext = dbContext;
        }
        public async Task<int> AddPhoto(ProductPhoto photo)
        {
            _dbContext.ProductPhotos.Add(photo);
            await _dbContext.SaveChangesAsync();
            return photo.Id;
        }

        public async Task<int> CheckSequence(int productId)
        {
            var result = await _dbContext.ProductPhotos.Where(p => p.ProductId == productId).OrderBy(p=>p.Id).LastOrDefaultAsync();
            if (result != null)
            {
                return result.SequenceNumber + 1;
            }
            else return 1;
        }

        public async Task<bool> DeletePhoto(int id)
        {
            var delete = _dbContext.ProductPhotos.First(p => p.Id == id);
            var lastInSequence = await _dbContext.ProductPhotos.Where(p => p.ProductId == delete.ProductId).OrderBy(p=>p.Id).LastAsync();

            if (delete != null)
            {
                if (delete.Id != lastInSequence.Id)
                {
                    lastInSequence.SequenceNumber = delete.SequenceNumber;
                }
                _dbContext.ProductPhotos.Remove(delete);
                int response = await _dbContext.SaveChangesAsync();
                var result = response>0 ?  true:  false;
                return result;
            }
            return false; 
        }

        public async Task<Dictionary<int, string?>> GetImagePathsByCategoryIdsAsync(IEnumerable<int> categoryIds)
        {
            var oldestProductPhotos = await _dbContext.Products
                .Where(p => categoryIds.Contains(p.CategoryId))
                .Join(_dbContext.ProductPhotos.Where(photo => photo.SequenceNumber == 1),
                    product => product.Id,
                    photo => photo.ProductId,
                    (product, photo) => new { product.CategoryId, photo.ImagePath, product.DateCreated }
                )
                .GroupBy(result => result.CategoryId)
                .ToDictionaryAsync(
                    group => group.Key,
                    group => group.OrderBy(result => result.DateCreated).FirstOrDefault()?.ImagePath
                );

            return oldestProductPhotos;
        }

        public async Task<string> GetNameFromUrl(int id)
        {
            var photo = await _dbContext.ProductPhotos.FirstOrDefaultAsync(p => p.Id == id);
            if (photo != null)
            {
                Uri uri = new Uri (photo.ImagePath);
                var result = uri.Segments.Last();
                return result;
            }
            return "";
        }
    }
}
