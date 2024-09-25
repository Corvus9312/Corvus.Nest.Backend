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

    public async Task<Category?> GetCategory(Guid? id = null, string? title = null, bool includeArticles = false)
    {
        var queryStr = "Select top 1 * from Category where 1 = 1 ";

        Dictionary<string, object> parameters = [];
        if (id is not null)
        {
            queryStr += " and ID = @ID ";
            parameters.Add("@ID", id);
        }
        if (!string.IsNullOrWhiteSpace(title))
        {
            queryStr += " and Title = @Title ";
            parameters.Add("@Title", title);
        }

        var result = (await database.SqlQueryAsync<Category>(queryStr, parameters)).FirstOrDefault();

        if (includeArticles && result is not null)
            result.Articles = (await GetArticles(result.ID)).ToList();

        return result;
    }

    public async Task<IEnumerable<Category>> GetCategories(bool includeArticles = false)
    {
        var queryStr = "Select * from Category ";

        var result = await database.SqlQueryAsync<Category>(queryStr);

        if (includeArticles)
        {
            foreach (var item in result)
            {
                item.Articles = (await GetArticles(item.ID)).ToList();
            }
        }

        return result;
    }

    public async Task<IEnumerable<Article>> GetArticles(Guid? id = null)
    {
        var queryStr = "Select * from Article ";

        return await database.SqlQueryAsync<Article>(queryStr);
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