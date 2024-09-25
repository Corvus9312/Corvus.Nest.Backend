using Corvus.Nest.Backend.Models.DAL.Corvus;
using Corvus.Nest.Backend.ViewModels;

namespace Corvus.Nest.Backend.Interfaces.IServices;

public interface IAppService
{
    Task<GetAboutVM> GetAbout();

    Task<IEnumerable<BlogMenu>> GetBlogMenus();
}