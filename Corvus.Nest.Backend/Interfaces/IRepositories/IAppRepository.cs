using Corvus.Nest.Backend.Models.DAL.Corvus;

namespace Corvus.Nest.Backend.Interfaces.IRepositories;

public interface IAppRepository
{
    Task<About> GetAbout();

    Task<IEnumerable<BlogMenu>> GetBlogMenus();

    Task<IEnumerable<SocialMedia>> GetSocialMedia();

    Task<Category?> GetCategory(Guid? id = null, string? title = null, bool includeArticles = false);

    Task<IEnumerable<Category>> GetCategories(bool includeArticles = false);

    Task<IEnumerable<Article>> GetArticles(Guid? id = null);

    Task<int> CreateArticle(Article article);

    Task<int> UpdateArticle(Article article);
}