namespace Kyvara.Builder.Generators.Application;

public sealed class ValidatorGenerator
{
    public string Generate()
    {
        return """
using FluentValidation;

namespace {{Namespace}}.Validators;

public sealed class Create{{Entity}}Validator
    : AbstractValidator<Create{{Entity}}Command>
{
    public Create{{Entity}}Validator()
    {
        RuleFor(x => x.Id)
            .NotEmpty();
    }
}

public sealed class Update{{Entity}}Validator
    : AbstractValidator<Update{{Entity}}Command>
{
    public Update{{Entity}}Validator()
    {
        RuleFor(x => x.Id)
            .NotEmpty();
    }
}
""";
    }
}
