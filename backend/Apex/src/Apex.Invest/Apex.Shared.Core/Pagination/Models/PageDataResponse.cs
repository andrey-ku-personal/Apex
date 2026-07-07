namespace Apex.Shared.Core.Pagination.Models;

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

    public int TotalCount { get; set; }

    public List<TData> Data { get; set; } = default!;
}