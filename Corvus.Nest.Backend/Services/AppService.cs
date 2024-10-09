using AutoMapper;
using Corvus.Nest.Backend.Extensions;
using Corvus.Nest.Backend.Interfaces.IRepositories;
using Corvus.Nest.Backend.Interfaces.IServices;
using Corvus.Nest.Backend.Models.DAL.Corvus;
using Corvus.Nest.Backend.ViewModels;

namespace Corvus.Nest.Backend.Services;

public class AppService(IAppRepository appRepository, IMapper mapper) : IAppService
{
    public async Task<GetAboutVM> GetAbout()
    {
        GetAboutVM result = mapper.Map<GetAboutVM>(await appRepository.GetAbout());

        result.SocialMedias = (await appRepository.GetSocialMedia()).ToList();

        return result;
    }

    public async Task<IEnumerable<BlogMenu>> GetBlogMenus() => await appRepository.GetBlogMenus();

    public async Task<Category?> GetCategory(Guid id)
    {
        var result = await appRepository.GetCategory(id);

        result?.Include(x => x.Articles);

        return result;
    }

    public async Task<IEnumerable<Category>> GetCategories()
    {
        var result = await appRepository.GetCategories();

        result.Include(x => x.Articles);

        return result;
    }

    public async Task<Article?> GetArticle(Guid id)
    {
        var result = await appRepository.GetArticle(id);

        result?.Include(x => x.CategoryNavigation);

        return result;
    }

    public async Task<IEnumerable<GetArticlesVM>> GetArticles(Guid? categoryID = null)
    {
        var result = await appRepository.GetArticles(categoryID);

        result.Include(x => x.CategoryNavigation);

        return result;
    }

    public async Task<int> CreateArticle(Article article)
    {
        article.ID = Guid.NewGuid();

        return await appRepository.CreateArticle(article);
    }
    
    public async Task<int> UpdateArticle(Article article)
    {
        return await appRepository.UpdateArticle(article);
    }
}