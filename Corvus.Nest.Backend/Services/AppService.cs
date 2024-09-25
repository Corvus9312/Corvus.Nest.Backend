using AutoMapper;
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

    public async Task<int> CreateArticle(Article article)
    {
        article.ID = Guid.NewGuid();

        return await appRepository.CreateArticle(article);
    }
}