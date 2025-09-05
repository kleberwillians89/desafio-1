using Hypesoft.Application;                 // tem o AssemblyMarker
using Hypesoft.Infrastructure.Data;
using Hypesoft.Infrastructure.Repositories;

using MediatR;
using AutoMapper;
using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

// Mongo
var mongoConn = builder.Configuration["Mongo:ConnectionString"] ?? "mongodb://localhost:27017";
var mongoDb   = builder.Configuration["Mongo:Database"]         ?? "hypesoft";
Console.WriteLine($"[Mongo] conn={mongoConn} | db={mongoDb}");

// Infra
builder.Services.AddSingleton(new MongoContext(mongoConn, mongoDb));
builder.Services.AddScoped<ICategoryRepository, CategoryRepository>();
builder.Services.AddScoped<IProductRepository, ProductRepository>();

// MediatR (v12) – registra Handlers/Requests do assembly da Application
// MediatR (v11)
builder.Services.AddMediatR(typeof(AssemblyMarker).Assembly);


// AutoMapper – perfis no assembly da Application
builder.Services.AddAutoMapper(typeof(AssemblyMarker).Assembly);

// MVC + FluentValidation
builder.Services.AddControllers();
// (estas duas extensões vêm do pacote FluentValidation.AspNetCore)
builder.Services
    .AddFluentValidationAutoValidation()
    .AddFluentValidationClientsideAdapters();
// registra validadores (CategoryValidators, ProductValidators, etc)
ValidatorOptions.Global.LanguageManager.Enabled = true;
builder.Services.AddValidatorsFromAssemblyContaining<AssemblyMarker>();

// Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "Hypesoft API", Version = "v1" });
});

// CORS
builder.Services.AddCors(opt =>
{
    opt.AddPolicy("web", p => p
        .WithOrigins("http://localhost:3000")
        .AllowAnyHeader()
        .AllowAnyMethod()
        .AllowCredentials());
});

// Auth (placeholder)
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer();

var app = builder.Build();

app.UseCors("web");

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthentication();
app.UseAuthorization();

app.MapGet("/health", () => Results.Ok(new { status = "ok" }));
app.MapControllers();

app.Run();
