using Corvus.Nest.Backend.Models.DAL.Corvus;
using Corvus.Nest.Backend.ViewModels;

namespace Corvus.Nest.Backend.Interfaces.IRepositories;

public interface IAppRepository
{
    Task<About> GetAbout();

    Task<IEnumerable<SocialMedia>> GetSocialMedia();

    Task<IEnumerable<BlogMenu>> GetBlogMenus();

    Task<Category?> GetCategory(Guid id);

    Task<Category?> GetCategory(string title);

    Task<IEnumerable<Category>> GetCategories();

    Task<Article?> GetArticle(Guid id);

    Task<Article?> GetArticle(string title);

    Task<IEnumerable<GetArticlesVM>> GetArticles(Guid? categoryID = null);

    Task<int> CreateArticle(Article article);

    Task<int> UpdateArticle(Article article);
}