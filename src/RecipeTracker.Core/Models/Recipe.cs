namespace RecipeTracker.Core.Models;

public record Recipe
{
    public required RecipeId Id { get; init; }
    public required string Name { get; set; }
    public required string Description { get; set; }
}

public readonly record struct RecipeId(int Value)
{
    public static implicit operator int(RecipeId id)
    {
        return id.Value;
    }

    public static explicit operator RecipeId(int id)
    {
        return new RecipeId(id);
    }

    public static explicit operator RecipeId(Int64 id)
    {
        return new RecipeId((int)id);
    }

    public override string ToString()
    {
        return this.Value.ToString();
    }
}