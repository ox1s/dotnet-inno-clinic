namespace Profile.Api.Contracts;

public static class Paging
{
    public const int DefaultPage = 1;
    public const int DefaultPageSize = 20;
    public const int MaxPageSize = 200;

    public static (int Page, int PageSize) Normalize(int page, int pageSize)
    {
        var normalizedPage = page < 1 ? DefaultPage : page;
        var normalizedPageSize = pageSize < 1 ? DefaultPageSize : pageSize;
        if (normalizedPageSize > MaxPageSize) normalizedPageSize = MaxPageSize;

        return (normalizedPage, normalizedPageSize);
    }
}

public sealed record PagedResponse<T>(
    IReadOnlyList<T> Items,
    int Page,
    int PageSize,
    int TotalCount);

