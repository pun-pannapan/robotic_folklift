namespace Robotic.Folklift.Application.Dtos
{
    public record PageQuery(int Page = 1, int Size = 20, string? SortBy = null, string SortDir = "asc");
    public class PagedResult<T>
    {
        public int Page { get; init; }
        public int Size { get; init; }
        public int TotalItems { get; init; }
        public IReadOnlyList<T> Items { get; init; } = Array.Empty<T>();
    }
}
