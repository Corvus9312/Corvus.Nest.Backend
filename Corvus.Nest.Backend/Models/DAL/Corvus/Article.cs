namespace Corvus.Nest.Backend.Models.DAL.Corvus;

public class Article
{
    public Guid ID { get; set; }

    public Guid Category { get; set; }

    public string Title { get; set; } = null!;

    public string Description { get; set; } = string.Empty;

    public string ArticleContent { get; set; } = null!;

    public int Sort { get; set; }

    public DateTime CreateTime { get; set; }
}
