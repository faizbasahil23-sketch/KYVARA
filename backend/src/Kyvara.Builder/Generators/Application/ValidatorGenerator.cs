namespace Kyvara.Builder.Generators.Application;

public sealed class ValidatorGenerator
{
    public string Generate()
    {
        return """
using FluentValidation;

namespace {{Namespace}}.Validators;

public sealed class Create{{Entity}}CommandValidator
    : AbstractValidator<Create{{Entity}}Command>
{
    public Create{{Entity}}CommandValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty();
    }
}
""";
    }
}
