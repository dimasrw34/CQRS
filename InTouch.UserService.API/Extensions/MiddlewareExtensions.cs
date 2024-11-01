using InTouch.UserService.Middlewares;
using Microsoft.AspNetCore.Builder;

namespace InTouch.UserService.Extensions;

internal static class MiddlewareExtensions
{
    public static void UseErrorHandling(this IApplicationBuilder builder) =>
        builder.UseMiddleware<ErrorHandlingMiddleware>();
}