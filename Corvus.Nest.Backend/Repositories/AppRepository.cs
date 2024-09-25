using Corvus.Nest.Backend.Interfaces.IHelpers;
using Corvus.Nest.Backend.Interfaces.IRepositories;
using Corvus.Nest.Backend.Models.DAL.Corvus;

namespace Corvus.Nest.Backend.Repositories;

public class AppRepository(IDatabaseHelper database) : IAppRepository
{
    public async Task<About> GetAbout()
    {
        var queryStr = @"select top 1 * from About ";

        return (await database.SqlQueryAsync<About>(queryStr)).First();
    }

    public async Task<IEnumerable<BlogMenu>> GetBlogMenus()
    {
        var queryStr = @"select * from BlogMenu order by Sort";

        return await database.SqlQueryAsync<BlogMenu>(queryStr);
    }

    public async Task<IEnumerable<SocialMedia>> GetSocialMedia()
    {
        var queryStr = @"select * from SocialMedia order by Sort";

        return await database.SqlQueryAsync<SocialMedia>(queryStr);
    }
}