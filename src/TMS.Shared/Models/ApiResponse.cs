using System.Collections.Generic;

namespace TMS.Shared.Models;

/// <summary>
/// Standard response envelope returned by every API endpoint, giving clients a
/// consistent shape for success and error results.
/// </summary>
public class ApiResponse<T>
{
    public bool Success { get; set; }
    public T? Data { get; set; }
    public string? Message { get; set; }
    public IEnumerable<string>? Errors { get; set; }
}

/// <summary>Factory helpers for building <see cref="ApiResponse{T}"/> instances.</summary>
public static class ApiResponse
{
    public static ApiResponse<T> Success<T>(T data, string? message = null)
        => new ApiResponse<T> { Success = true, Data = data, Message = message };

    public static ApiResponse<T> Failure<T>(string message, IEnumerable<string>? errors = null)
        => new ApiResponse<T> { Success = false, Message = message, Errors = errors };
}
