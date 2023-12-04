namespace PulseStore.BLL.Entities;

public class SearchHistoryItem
{
    public int Id { get; set; }
    public string UserId { get; set; }
    public string Query { get; set; }
    public DateTime Date { get; set; }
}