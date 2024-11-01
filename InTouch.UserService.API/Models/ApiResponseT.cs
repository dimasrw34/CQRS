using System.Collections.Generic;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Http;

namespace InTouch.UserService.Models;

public class ApiResponse<T> : ApiResponse
{
    public ApiResponse()
    {
    }
    
    [JsonConstructor]
    public ApiResponse(
        T result, 
        bool success, 
        string successMessage, 
        int statusCode,
        IEnumerable<ApiErrorResponse> errors) 
        : base(success,successMessage,statusCode,errors)
    {
        Result = result;
    }

    public T Result { get; private init; }

    public static ApiResponse<T> Ok(T result) =>
        new() { Success = true, StatusCode = StatusCodes.Status200OK, Result = result};
    
    public static ApiResponse<T> Ok(T result, string successMessage) =>
        new() { Success = true, StatusCode = StatusCodes.Status200OK, Result = result, SuccessMessage = successMessage};
}