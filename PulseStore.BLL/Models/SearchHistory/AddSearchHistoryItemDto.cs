namespace PulseStore.BLL.Models.SearchHistory;

public record AddSearchHistoryItemDto(string Query)
{
    public string? UserId { get; set; }
    public DateTime Date { get; set; }
}