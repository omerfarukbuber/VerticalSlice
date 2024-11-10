namespace Newsletter.API.Shared;

public sealed record Error(string Code, string? Description = null)
{
    public static readonly Error None = new Error(string.Empty);
}