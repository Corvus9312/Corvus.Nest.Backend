using Corvus.Nest.Backend.Interfaces.IHelpers;
using Corvus.Nest.Backend.Interfaces.IRepositories;
using Corvus.Nest.Backend.Models.DAL.Corvus;
using Corvus.Nest.Backend.ViewModels;

namespace Corvus.Nest.Backend.Repositories;

public class AppRepository(IDatabaseHelper database) : IAppRepository
{
    public async Task<About> GetAbout()
    {
        var queryStr = @"select top 1 * from About ";

        return (await database.SqlQueryAsync<About>(queryStr)).First();
    }

    public async Task<IEnumerable<SocialMedia>> GetSocialMedia()
    {
        var queryStr = @"select * from SocialMedia order by Sort";

        return await database.SqlQueryAsync<SocialMedia>(queryStr);
    }

    public async Task<IEnumerable<BlogMenu>> GetBlogMenus()
    {
        var queryStr = @"select * from BlogMenu order by Sort";

        return await database.SqlQueryAsync<BlogMenu>(queryStr);
    }

    public async Task<Category?> GetCategory(Guid id) => await GetCategory(id, null);

    public async Task<Category?> GetCategory(string title) => await GetCategory(null, title);

    public async Task<Category?> GetCategory(Guid? id = null, string? title = null)
    {
        var queryStr = "Select top 1 * from Category where 1 = 1 ";

        Dictionary<string, object> parameters = [];
        if (id is not null)
        {
            queryStr += @" and ID = @ID ";
            parameters.Add("@ID", id);
        }
        else if (title is not null)
        {
            queryStr += @" and Title = @Title ";
            parameters.Add("@Title", title);
        }

        return (await database.SqlQueryAsync<Category>(queryStr, parameters)).FirstOrDefault();
    }

    public async Task<IEnumerable<Category>> GetCategories()
    {
        var queryStr = "select * from Category ";

        var result = await database.SqlQueryAsync<Category>(queryStr);

        return result;
    }

    public async Task<Article?> GetArticle(Guid id) => await GetArticle(id, null);

    public async Task<Article?> GetArticle(string title) => await GetArticle(null, title);

    private async Task<Article?> GetArticle(Guid? id = null, string? title = null)
    {
        var queryStr = @"select top 1 * from Article where 1 = 1 ";

        Dictionary<string, object> parameters = [];
        if (id is not null)
        {
            queryStr += " and ID = @ID ";
            parameters.Add("ID", id);
        }
        else if (title is not null)
        {
            queryStr += " and Title = @Title ";
            parameters.Add("Title", title);
        }

        return (await database.SqlQueryAsync<Article>(queryStr, parameters)).FirstOrDefault();
    }

    public async Task<IEnumerable<GetArticlesVM>> GetArticles(Guid? categoryID = null)
    {
        var queryStr = @"select ID, Category, Title, 
                            Description, Sort, CreateTime 
                        from Article ";

        if (categoryID is not null)
            queryStr += " where Category = @Category ";

        return await database.SqlQueryAsync<GetArticlesVM>(queryStr, new { Category = categoryID });
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