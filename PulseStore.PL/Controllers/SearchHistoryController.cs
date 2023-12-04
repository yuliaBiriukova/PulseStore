using AutoMapper;
using PulseStore.BLL.Models.SearchHistory;
using PulseStore.BLL.Services.SearchHistory;
using PulseStore.PL.Extensions;
using PulseStore.PL.ViewModels.SearchHistory;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace PulseStore.PL.Controllers
{
    [Route("api/search-history")]
    [ApiController]
    [Authorize(Policy = "RequireAuthorizedUser")]
    public class SearchHistoryController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly ISearchHistoryService _searchHistoryService;

        public SearchHistoryController(IMapper mapper, ISearchHistoryService searchHistoryService)
        {
            _mapper = mapper;
            _searchHistoryService = searchHistoryService;
        }

        /// <summary>
        ///     Adds search history item with search guery from <see cref="AddSearchHistoryViewModel"/>.
        /// </summary>
        /// <param name="model"><see cref="AddSearchHistoryViewModel"/> entity with search query to add.</param>
        /// <remarks>
        ///     Sets current userId and <see cref="DateTime"/>.
        /// </remarks>
        /// <returns>
        ///     HTTP 200 OK with Id of the added entity.
        /// </returns>
        [HttpPost]
        public async Task<ActionResult<int>> AddSearchHistoryItem(AddSearchHistoryViewModel model)
        {
            var userId = User.GetUserId().ToString();
            var newSearchHistoryItem = _mapper.Map<AddSearchHistoryItemDto>(model);
            newSearchHistoryItem.UserId = userId;
            newSearchHistoryItem.Date = DateTime.Now;

            var id = await _searchHistoryService.UpsertAsync(newSearchHistoryItem);
            return Ok(id);
        }

        /// <summary>
        ///     Deletes all search history by userId.
        /// </summary>
        /// <returns>
        ///     HTTP 200 OK
        /// </returns>
        [HttpDelete]
        public async Task<ActionResult> DeleteAllSearchHistory()
        {
            var userId = User.GetUserId().ToString();
            await _searchHistoryService.DeleteAllAsync(userId);
            return Ok();
        }

        /// <summary>
        ///     Deletes search history item by id.
        /// </summary>     
        /// <returns>
        ///     HTTP 200 OK
        /// </returns>
        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteSearchHistoryItemById(int id)
        {
            await _searchHistoryService.DeleteAsync(id);
            return Ok();
        }

        /// <summary>
        ///     Gets search history by userId.
        /// </summary>     
        /// <returns>
        ///    HTTP 200 OK with a list of user search history items.
        /// </returns>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<SearchHistoryViewModel>>> GetSearchHistory()
        {
            var userId = User.GetUserId().ToString();
            var result = await _searchHistoryService.GetAsync(userId);
            return Ok(_mapper.Map<IEnumerable<SearchHistoryViewModel>>(result));
        }
    }
}