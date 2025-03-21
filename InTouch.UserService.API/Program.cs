using System.Globalization;
using FluentValidation;
using FluentValidation.Resources;
using InTouch.Application;
using InTouch.Infrastructure;
using InTouch.UserService.Infrastructure.Authentification;
using InTouch.UserService;
using InTouch.UserService.Core;
using InTouch.UserService.Extensions;
using InTouch.UserService.Query;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using System.IO;
using System;

var builder = WebApplication.CreateBuilder(args);
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


//настройка Swagger
builder.Services.AddSwaggerGen(op =>
{
    var xmlPath = Path.Combine(AppContext.BaseDirectory, "InTouch.UserService.API.xml");
    op.IncludeXmlComments(xmlPath);
    op.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        In = ParameterLocation.Header,
        Description = "Авторизация",
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        BearerFormat = "JWT",
        Scheme = "Bearer"
    });

    op.SwaggerDoc("v1", new OpenApiInfo { Title = "InTouch.UserService API", Version = "v0.0.1" });
    op.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type=ReferenceType.SecurityScheme,
                    Id="Bearer"
                }
            },
            new string[]{}
        }
    });
});

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
