using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;

namespace ECommerceApi.Application.Wrapper;

public class PagedResponse<T>
{
    public T Data { get; set; }
    public int PageNumber { get; set; }
    public int PageSize { get; set; }
    public int TotalPages { get; set; }
    public int TotalRecords { get; set; }
    public bool Succeeded { get; set; } 
    public string Message { get; set; }
    public string[] Errors { get; set; }

    public PagedResponse(T data, int pageNumber, int pageSize, int totalRecords)
    {
        Data = data;
        PageNumber = pageNumber;
        PageSize = pageSize;
        TotalRecords = totalRecords;

        TotalPages = (int)Math.Ceiling(totalRecords / (double)pageSize);

        Succeeded = true;
        Message = null;
        Errors = null;
    }
}
