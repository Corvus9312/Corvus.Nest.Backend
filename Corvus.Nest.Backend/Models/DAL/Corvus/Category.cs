namespace Corvus.Nest.Backend.Models.DAL.Corvus;

public class Category
{
    public Guid ID { get; set; }

    public string Title { get; set; } = null!;

    public IEnumerable<Article> Articles { get; set; } = [];
}