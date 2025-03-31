using System.Globalization;
using FluentValidation;
using FluentValidation.Resources;
using InTouch.UserService.Application;
using InTouch.UserService.Infrastructure.Data;
using InTouch.UserService.Infrastructure.Authentification;
using InTouch.UserService;
using InTouch.UserService.Core;
using InTouch.UserService.Extensions;
using InTouch.UserService.Query;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

var builder = WebApplication.CreateBuilder(args);

builder.Logging   
    .ClearProviders()
     .AddSimpleConsole(options =>
    {
        options.IncludeScopes = true;
        options.SingleLine = true;
        options.TimestampFormat = "HH:mm:ss "; // Работает в SimpleConsole
    });

var configuration = builder.Configuration;

builder.Services.Configure<JsonOptions>(jsonOptions => jsonOptions.JsonSerializerOptions.Configure());
builder.Services.Configure<JwtOptions>(configuration.GetSection(nameof(JwtOptions)));
builder.Services.AddAuthentificationAndAuthorization(configuration);

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
builder.Services.AddCacheService(configuration);

builder.Logging.ClearProviders();
builder.Logging.AddConsole();

//настройка Swagger
builder.Services.AddSwagger();


// FluentValidation global configuration.
ValidatorOptions.Global.DisplayNameResolver = (_, member, _) => member?.Name;
ValidatorOptions.Global.LanguageManager = new LanguageManager { Enabled = true, Culture = new CultureInfo("ru-ru") };


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseErrorHandling();
app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();
app.Run();
