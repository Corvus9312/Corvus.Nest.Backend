using Corvus.Nest.Backend.Interfaces.IRepositories;
using Corvus.Nest.Backend.Interfaces.IServices;
using Corvus.Nest.Backend.Models.DAL.Corvus;
using Corvus.Nest.Backend.Repositories;
using Corvus.Nest.Backend.Services;

namespace Corvus.Nest.Backend;

public class Program
{
    private static IAppService _appService = null!;

    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        builder.Services.AddSingleton<IAppService, AppService>();
        builder.Services.AddSingleton<IAppRepository, AppRepository>();

        ProgramBase.BaseBuilder(
            (app, scope) =>
            {
                _appService = scope.ServiceProvider.GetRequiredService<IAppService>();

                app.Map("/GetAbout", () => GetAbout());

                app.Map("/GetBlogMenus", () => GetBlogMenus());

                app.MapPost("/CreateArticle",
                    (Article article) => CreateArticles(article));

            }, builder);
    }

    private static async Task<IResult> GetAbout() => Results.Ok(await _appService.GetAbout());
    
    private static async Task<IResult> GetBlogMenus() => Results.Ok(await _appService.GetBlogMenus());

    private static async Task<IResult> CreateArticles(Article article) => Results.Ok(await _appService.CreateArticle(article));
}