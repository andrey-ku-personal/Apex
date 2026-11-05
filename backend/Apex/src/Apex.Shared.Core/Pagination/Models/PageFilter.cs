using Apex.Shared.Core.Pagination.Abstract;

namespace Apex.Shared.Core.Pagination.Models;

public class PageFilter : IPageSortQuery
{
    public int PageNumber { get; set; } = 0;
    public int PageSize { get; set; } = 10;
    public string SortBy { get; set; } = "Id";
    public bool IsAscending { get; set; }
    public string? FreeText { get; set; }
}