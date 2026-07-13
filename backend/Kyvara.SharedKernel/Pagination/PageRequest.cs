namespace Kyvara.SharedKernel.Pagination;

public sealed class PageRequest
{
    public int Page { get; init; } = 1;

    public int Size { get; init; } = 20;

    public int Skip =>
        (Page-1)*Size;
}
