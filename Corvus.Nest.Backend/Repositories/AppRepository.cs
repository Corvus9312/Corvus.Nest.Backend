using Corvus.Nest.Backend.Interfaces.IHelpers;
using Corvus.Nest.Backend.Interfaces.IRepositories;
using Corvus.Nest.Backend.Models.DAL.Corvus;
using Corvus.Nest.Backend.Extensions;

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

    public async Task<Category?> GetCategory(Guid id)
    {
        var queryStr = "Select top 1 * from Category where and ID = @ID ";

        Dictionary<string, object> parameters = [];
        parameters.Add("@ID", id);

        return (await database.SqlQueryAsync<Category>(queryStr, parameters)).FirstOrDefault();
    }

    public async Task<IEnumerable<Category>> GetCategories()
    {
        var queryStr = "select * from Category ";

        var result = await database.SqlQueryAsync<Category>(queryStr);

        return result;
    }

    public async Task<IEnumerable<Article>> GetArticles(Guid? category = null)
    {
        var queryStr = "select * from Article ";

        if (category is not null)
            queryStr += " where Category = @Category ";

        return await database.SqlQueryAsync<Article>(queryStr, new { Category = category });
    }

    public async Task<int> CreateArticle(Article article)
    {
        var sqlStr = @"insert into Article 
                        (
                            ID, Category, Title, Description,
                            ArticleContent, Sort, CreateTime
                        )values (
                            @ID, @Category, @Title, @Description,
                            @ArticleContent, @Sort, @CreateTime
                        )";

        return await database.SqlNonQueryAsync(sqlStr, article);
    }

    public async Task<int> UpdateArticle(Article article)
    {
        var sqlStr = @"Update Article
                        set Category = @Category, Title = @Title, Description = @Description,
                            ArticleContent = @ArticleContent, Sort = @Sort
                        where ID = @ID";

        return await database.SqlNonQueryAsync(sqlStr, article);
    }
}