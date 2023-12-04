using PulseStore.BLL.Entities;

namespace PulseStore.BLL.Repositories
{
    public interface IPhotoRepository
    {
        /// <summary>
        ///     Check is the <see cref="ProductPhoto"/> is first photo of <see cref="Product"/>.
        /// </summary>
        /// <param name="productId"><see cref="int"/> id of <see cref="Product"/>.</param>
        /// <returns>
        ///     Returns sequence number of photo that should be added to database as <see cref="int"/>.
        /// </returns>
        Task<int> CheckSequence(int productId);

        /// <summary>
        ///     Addes <see cref="ProductPhoto"/> to Database.
        /// </summary>
        /// <param name="photo"><see cref="ProductPhoto"/> one of the entities of the database.</param>
        /// <returns>
        ///     Returns <see cref="ProductPhoto.Id"/> as <see cref="int"/>, of entity that was added
        /// </returns>
        Task<int> AddPhoto(ProductPhoto photo);

        /// <summary>
        ///     Delete photo from database by <paramref name="id"/>.
        /// </summary>
        /// <param name="id"> Id of photo which should be deleted.</param>
        /// <returns>
        ///     Returns result of delete as <see cref="bool"/>.
        /// </returns>
        Task<bool> DeletePhoto(int id);

        /// <summary>
        ///     Gets image paths of the oldest products in categories with specified ids.
        /// </summary>
        /// <param name="categoryIds">List of categoty ids which photos to get.</param>
        /// <returns>
        ///     Key-value pairs of category id and image path.
        /// </returns>
        Task<Dictionary<int, string?>> GetImagePathsByCategoryIdsAsync(IEnumerable<int> categoryIds);

        /// <summary>
        ///     Return name of photo from <see cref="ProductPhoto.ImagePath"/>.
        /// </summary>
        /// <param name="id"> Id of photo which should be deleted.</param>
        /// <returns>
        ///     Returns name of <see cref="ProductPhoto"/> file as <see cref="string"/>.
        /// </returns>
        Task<string> GetNameFromUrl(int id);
    }
}