using Corvus.Nest.Backend.Models.DAL.Corvus;

namespace Corvus.Nest.Backend.Interfaces.IRepositories;

public interface IAppRepository
{
    Task<About> GetAbout();

    Task<IEnumerable<BlogMenu>> GetBlogMenus();

    Task<IEnumerable<SocialMedia>> GetSocialMedia();
}