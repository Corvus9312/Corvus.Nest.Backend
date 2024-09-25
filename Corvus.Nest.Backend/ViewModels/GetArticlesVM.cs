using Corvus.Nest.Backend.Models.DAL.Corvus;

namespace Corvus.Nest.Backend.ViewModels;

public class GetArticlesVM
{
    public Guid ID { get; set; }

    public Guid Category { get; set; }

    public string Title { get; set; } = null!;

    public string Description { get; set; } = string.Empty;

    public int Sort { get; set; }

    public DateTime CreateTime { get; set; }

    public Category CategoryNavigation { get; set; } = null!;
}