namespace Corvus.Nest.Backend.Models.DAL.Corvus;

public static class Context
{
    public static List<RelationalModel> Relationals { get; private set; } =
        [
            new()
            {
                Primary = new(){ Name = "Category", Key = "ID" },
                Foreign = new(){ Name = "Article", Key = "Category" },
                Type = typeof(Guid)
            }
        ];

}

public class RelationalModel
{
    public RelationalDetail Primary { get; set; } = null!;
    public RelationalDetail Foreign { get; set; } = null!;
    public Type Type { get; set; } = null!;
}

public class RelationalDetail
{
    public string Name { get; set; } = null!;

    public string Key { get; set; } = null!;
}