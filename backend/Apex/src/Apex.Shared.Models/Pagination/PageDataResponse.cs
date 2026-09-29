namespace Apex.Shared.Models.Pagination;

public class PageDataResponse<TData>
    where TData : class
{
    public PageDataResponse()
    {
    }

    public PageDataResponse(int totalCount, List<TData> data)
    {
        TotalCount = totalCount;
        Data = data;
    }

    public int TotalCount { get; }

    public List<TData> Data { get; } = default!;
}
