using System;
using InTouch.UserService.Core;
using Microsoft.Extensions.DependencyInjection;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace InTouch.UserService;

//чтобы swagger правильно обрабатывал DateOnly (пока не используется)
public static class SwaggerGenOptionsExtensions
{
    public static void UseStringConverter(this SwaggerGenOptions options)
    {
        options.MapType<DateOnly>(() => new Microsoft.OpenApi.Models.OpenApiSchema
        {
            Type = "string",
            Format = "date"
        });
        options.MapType<TimeOnly>(() => new Microsoft.OpenApi.Models.OpenApiSchema
        {
            Type = "string",
            Format = "time",
            Example = OpenApiAnyFactory.CreateFromJson("\"13:45:45.0000000\"")
        });
    }
}
