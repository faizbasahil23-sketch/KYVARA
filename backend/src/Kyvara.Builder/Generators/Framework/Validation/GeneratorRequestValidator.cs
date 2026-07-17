using Kyvara.Builder.Generators.Framework.Core;
using Kyvara.Builder.Generators.Framework.Models;

namespace Kyvara.Builder.Generators.Framework.Validation;

public static class GeneratorRequestValidator
{
    public static void Validate(GeneratorRequest request)
    {
        ArgumentNullException.ThrowIfNull(request);

        if (string.IsNullOrWhiteSpace(request.GeneratorName))
        {
            throw new GeneratorException(
                "Generator name is required.");
        }

        if (string.IsNullOrWhiteSpace(request.OutputDirectory))
        {
            throw new GeneratorException(
                "Output directory is required.");
        }

        ValidateTokens(request.Tokens);
    }

    private static void ValidateTokens(
        IReadOnlyDictionary<string, string> tokens)
    {
        ArgumentNullException.ThrowIfNull(tokens);

        foreach (var token in tokens)
        {
            if (string.IsNullOrWhiteSpace(token.Key))
            {
                throw new GeneratorException(
                    "Generator token names cannot be empty.");
            }

            if (token.Value is null)
            {
                throw new GeneratorException(
                    $"Generator token '{token.Key}' cannot be null.");
            }
        }
    }
}
