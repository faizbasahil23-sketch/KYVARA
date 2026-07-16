namespace Kyvara.Builder.Generators.Application;

public sealed class DtoGenerator
{
    public string Generate()
    {
        return """
namespace {{Namespace}}.DTOs;

public sealed record {{Entity}}Dto(
    Guid Id
);

public sealed record Create{{Entity}}Dto();

public sealed record Update{{Entity}}Dto(
    Guid Id
);

public sealed record {{Entity}}SummaryDto(
    Guid Id
);

public sealed record {{Entity}}DetailDto(
    Guid Id
);
""";
    }
}
