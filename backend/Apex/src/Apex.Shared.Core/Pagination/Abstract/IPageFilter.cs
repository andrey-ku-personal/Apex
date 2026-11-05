namespace Apex.Shared.Core.Pagination.Abstract;

public interface IPageFilter
{
    public int PageNumber { get; set; }
    public int PageSize { get; set; }
}