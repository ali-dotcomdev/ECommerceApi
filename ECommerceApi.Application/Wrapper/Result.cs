using System;
using System.Collections.Generic;
using System.Text;

namespace ECommerceApi.Application.Wrapper;

public class Result<T>
{
    public string Message { get; set; }
    public bool Succeeded { get; set; }
    public T Data { get; set; }
    public List<string> Errors { get; set; }

    public Result()
    {

    }

    public Result(T data, string message = null)
    {
        Succeeded = true;
        Message = message;
        Data = data;
        Errors = null;
    }
    
    public Result(string message)
    {
        Succeeded = false;
        Message = message;
        Errors = new List<string> { message };
    }

    public static Result<T> Success(T data, string message = "islem basarili")
    {
        return new Result<T>(data, message);
    }

    public static Result<T> Fail(string message, List<string> errors = null)
    {
        return new Result<T>
        {
            Succeeded = false,
            Message = message,
            Errors = errors ?? new List<string> { message }
        };
    }
}
