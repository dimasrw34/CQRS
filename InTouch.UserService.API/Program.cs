using System.Globalization;
using FluentValidation;
using FluentValidation.Resources;
using InTouch.Application;
using InTouch.Infrastructure;
using InTouch.UserService;
using InTouch.UserService.Core;
using InTouch.UserService.Extensions;
using InTouch.UserService.Query;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);


builder.Services.Configure<JsonOptions>(jsonOptions => jsonOptions.JsonSerializerOptions.Configure());

builder.Services
    .AddEndpointsApiExplorer()
    .AddControllers()
    .AddJsonOptions(_ => { });

builder.Services.ConfigureAppSettings();
builder.Services.AddRegisterTypeHandler();
builder.Services.AddInfrastructure();
builder.Services.AddCommandHandlers();
builder.Services.AddQueryHandlers();
builder.Services.AddWriteOnlyRepositories();
builder.Services.AddResponseMediatr();
builder.Services.AddReadDbContext();
builder.Services.AddReadOnlyRepositories();
builder.Services.AddCacheService(builder.Configuration);

builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "API", Version = "v1" });
});

// FluentValidation global configuration.
ValidatorOptions.Global.DisplayNameResolver = (_, member, _) => member?.Name;
ValidatorOptions.Global.LanguageManager = new LanguageManager { Enabled = true, Culture = new CultureInfo("en-US") };


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseErrorHandling();
app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
app.Run();
