using Swashbuckle.AspNetCore.SwaggerGen;

namespace WebApplication1;

//чтобы swagger правильно обрабатывал DateOnly
public static class SwaggerGenOptionsExtensions
{
    public static void UseDateOnlyTimeOnlyStringConverter(this SwaggerGenOptions options)
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
