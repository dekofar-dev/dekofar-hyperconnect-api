public class PagedResult<T>
{
    public List<T> Items { get; set; } = new();
    public string? NextPageInfo { get; set; }
}
