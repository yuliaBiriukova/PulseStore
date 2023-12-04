using PulseStore.BLL.Entities.TemplateFiles.Enums;

namespace PulseStore.BLL.Entities.TemplateFiles;

public class TemplateFile
{
    public int Id { get; set; }
    public TemplateFileType Type { get; set; }
    public string FilePath { get; set; }
}