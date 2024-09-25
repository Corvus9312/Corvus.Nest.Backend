using Corvus.Nest.Backend.Helpers;
using Corvus.Nest.Backend.Interfaces.IHelpers;
using Corvus.Nest.Backend.Middlewares;
using Corvus.Nest.Backend.Middlewares.Tools;
using Microsoft.AspNetCore.Http.Json;
using System.Text.Json.Serialization;

namespace Corvus.Nest.Backend;

public static class ProgramBase
{
    public static void BaseBuilder(
        Action<WebApplication, IServiceScope> action,
        WebApplicationBuilder builder) 
    {
        var services = builder.Services;
        var configuration = builder.Configuration;

        services.AddCors();

        // jsonOption
        builder.Services.Configure<JsonOptions>(options =>
        {
            options.SerializerOptions.PropertyNamingPolicy = null;
            options.SerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
        });

        services.AddSingleton<IDatabaseHelper, DatabaseHelper>();
        services.AddSingleton<IFileHelper, FileHelper>();
        services.AddSingleton<IHttpClientHelper, HttpClientHelper>();
        services.AddSingleton<IEncryptionHelper, EncryptionHelper>();

        services.AddScoped<ILogIDs, LogIDs>();

        #region AutoMapper
        services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
        #endregion

        var app = builder.Build();

        app.UseCors(cors => cors
            .AllowAnyMethod()
            .AllowAnyHeader()
            .SetIsOriginAllowed(origin => true)
            .AllowCredentials()
        );

        app.UseHsts();
        app.UseRouting();

        app.UseMiddleware<LoggingMiddleware>();

        using IServiceScope scope = app.Services.CreateScope();

        action(app, scope);

        app.Run();
    }
}