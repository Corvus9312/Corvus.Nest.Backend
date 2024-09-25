using Corvus.Nest.Backend.Models.DAL.Corvus;
using Corvus.Nest.Backend.ViewModels;

namespace Corvus.Nest.Backend.Interfaces.IServices;

public interface IAppService
{
    Task<GetAboutVM> GetAbout();

    Task<IEnumerable<BlogMenu>> GetBlogMenus();

    Task<Category?> GetCategory(Guid id);

    Task<IEnumerable<Category>> GetCategories();

    Task<Article?> GetArticle(Guid id);

    Task<IEnumerable<GetArticlesVM>> GetArticles(Guid? categoryID = null);

    Task<int> CreateArticle(Article article);

    Task<int> UpdateArticle(Article article);
}