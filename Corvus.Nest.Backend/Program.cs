using Corvus.Nest.Backend.Interfaces.IRepositories;
using Corvus.Nest.Backend.Interfaces.IServices;
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

            }, builder);
    }

    private static async Task<IResult> GetAbout() => Results.Ok(await _appService.GetAbout());
    
    private static async Task<IResult> GetBlogMenus() => Results.Ok(await _appService.GetBlogMenus());
}
