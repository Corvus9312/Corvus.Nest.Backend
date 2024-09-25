using Corvus.Nest.Backend.Models.DAL.Corvus;

namespace Corvus.Nest.Backend.ViewModels;

public class GetAboutVM : About
{
    public List<SocialMedia> SocialMedias { get; set; } = [];
}