using HotelOpt.Domain.Common;

namespace HotelOpt.Application.Pagination;

public class PaginatedResult<T> 
{
    public List<T> Items { get; set; }
    public  int TotalCount { get; set; }
    public  int PageSize { get; set; }
    public int Page { get; set; }
    public int TotalPages { get; set; }

    public PaginatedResult(List<T> items, int totalCount, int pageSize, int page)
    {
        Items = items;
        TotalCount = totalCount;
        PageSize = pageSize;
        Page = page;
        TotalPages = (int)Math.Ceiling((double)TotalCount / PageSize);
    }
    
}