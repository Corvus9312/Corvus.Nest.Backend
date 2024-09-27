using Corvus.Nest.Backend.Extensions;
using Corvus.Nest.Backend.Interfaces.IRepositories;
using Corvus.Nest.Backend.Interfaces.IServices;
using Corvus.Nest.Backend.Models.DAL.Corvus;
using Corvus.Nest.Backend.Repositories;
using Corvus.Nest.Backend.Services;

namespace Corvus.Nest.Backend;

public class Program
{
    private static IAppService _appService = null!;
    public static IAppRepository AppRepository { get; private set; } = null!;

    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        builder.Services.AddSingleton<IAppService, AppService>();
        builder.Services.AddSingleton<IAppRepository, AppRepository>();

        ProgramBase.BaseBuilder(
            (app, scope) =>
            {
                _appService = scope.ServiceProvider.GetRequiredService<IAppService>();
                AppRepository = scope.ServiceProvider.GetRequiredService<IAppRepository>();

                app.MapGet("/GetAbout", async () => await GetAbout());

                app.MapGet("/GetBlogMenus", async () => await GetBlogMenus());

                app.MapGet("/GetArticles", async () => await GetArticles());

                app.MapPost("/CreateArticle",
                    async (Article article) => await CreateArticles(article));

            }, builder);
    }

    private static async Task<IResult> GetAbout() => Results.Ok(await _appService.GetAbout());

    private static async Task<IResult> GetBlogMenus() => Results.Ok(await _appService.GetBlogMenus());

    private static async Task<IResult> GetArticles() => Results.Ok();

    private static async Task<IResult> CreateArticles(Article article) => Results.Ok(await _appService.CreateArticle(article));
}