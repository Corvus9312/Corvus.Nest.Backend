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

                app.MapGet("/GetCategory", async (Guid id) => await GetCategory(id));

                app.MapGet("/GetCategories", async () => await GetCategories());

                app.MapGet("/GetArticle", async (Guid id) => await GetArticle(id));

                app.MapGet("/GetArticles", async (Guid? categoryID) => await GetArticles(categoryID));

                app.MapPost("/CreateArticle",
                    async (Article article) => await CreateArticles(article));

                app.MapPost("/UpdateArticle",
                    async (Article article) => await UpdateArticle(article));
            }, builder);
    }

    private static async Task<IResult> GetAbout() => Results.Ok(await _appService.GetAbout());

    private static async Task<IResult> GetBlogMenus() => Results.Ok(await _appService.GetBlogMenus());

    private static async Task<IResult> GetCategory(Guid id) => Results.Ok(await _appService.GetCategory(id));

    private static async Task<IResult> GetCategories() => Results.Ok(await _appService.GetCategories());

    private static async Task<IResult> GetArticle(Guid id) => Results.Ok(await _appService.GetArticle(id));

    private static async Task<IResult> GetArticles(Guid? categoryID) => Results.Ok(await _appService.GetArticles(categoryID));

    private static async Task<IResult> CreateArticles(Article article) => Results.Ok(await _appService.CreateArticle(article));

    private static async Task<IResult> UpdateArticle(Article article) => Results.Ok(await _appService.UpdateArticle(article));
}