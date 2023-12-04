using PulseStore.BLL.Services.TemplateFile;
using PulseStore.PL.ViewModels.TemplateFile;
using Microsoft.AspNetCore.Mvc;

namespace PulseStore.PL.Controllers.AdminControllers
{
    [Route("api/admin/template-files")]
    [ApiExplorerSettings(GroupName = "Admin/TemplateFiles")]
    [ApiController]
    public class TemplateFilesController : ControllerBase
    {
        private readonly ITemplateFileService _templateFileService;

        public TemplateFilesController(ITemplateFileService templateFileService)
        {
            _templateFileService = templateFileService;
        }

        [HttpPost]
        public async Task<ActionResult<int>> AddFile([FromForm] AddTemplateFileViewModel model)
        {
            var result = await _templateFileService.UpsertAsync(model.TemplateFile, model.TemplateType);
            return Ok(result);
        }
    }
}