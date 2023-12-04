using PulseStore.BLL.Entities.TemplateFiles.Enums;

namespace PulseStore.PL.ViewModels.TemplateFile;

public record AddTemplateFileViewModel(
    IFormFile TemplateFile,
    TemplateFileType TemplateType);