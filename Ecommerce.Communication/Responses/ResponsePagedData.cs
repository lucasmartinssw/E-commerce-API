using System.Collections.Generic;

namespace Ecommerce.Communication.Responses;

public class ResponsePagedData<T>
{
    public List<T> Data { get; set; } = new List<T>();
    public int CurrentPage { get; set; }
    public int TotalPages { get; set; }
    public int PageSize { get; set; }
    public int TotalCount { get; set; }

    public bool HasPrevious => CurrentPage > 1;
    public bool HasNext => CurrentPage < TotalPages;
}