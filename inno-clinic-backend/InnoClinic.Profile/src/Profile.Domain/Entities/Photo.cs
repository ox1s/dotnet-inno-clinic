namespace Profile.Domain.Entities;

public record Photo(string Url, string PublicId)
{
    public IEnumerable<(string Name, object Value)> GetValueComponents()
    {
        yield return (nameof(Url), Url);
        yield return (nameof(PublicId), PublicId);
    }
}