namespace Apex.Shared.Core.Pagination.Abstract;

public interface IPageSortQuery : IPageFilter
{
    public string SortBy { get; set; }
    public bool IsAscending { get; set; }
}
