using System.Text.RegularExpressions;
using Kyvara.Builder.Generators.Application.Core;
using Kyvara.Builder.Generators.Framework.Abstractions;
using Kyvara.Builder.Generators.Framework.Core;
using Kyvara.Builder.Generators.Framework.Models;
using Kyvara.Builder.Generators.Framework.Templates;

namespace Kyvara.Builder.Generators.Application.Commands;

/// <summary>
/// Generates an application command record.
/// </summary>
public sealed partial class ApplicationCommandGenerator
    : ApplicationGeneratorBase
{
    private const string TemplateName = "application/command.cs";

    private const string DefaultOutputPath = "Commands";

    private static readonly IReadOnlyDictionary<string, string>
        DefaultTemplates =
            new Dictionary<string, string>(
                StringComparer.OrdinalIgnoreCase)
            {
                [TemplateName] =
                    """
                    namespace {{Namespace}};

                    /// <summary>
                    /// Represents the {{CommandName}} application command.
                    /// </summary>
                    public sealed record {{CommandName}};
                    """
            };

    private readonly TemplateEngine _templateEngine;

    /// <summary>
    /// Creates the generator with its built-in command template.
    /// </summary>
    public ApplicationCommandGenerator()
        : this(CreateDefaultTemplateEngine())
    {
    }

    /// <summary>
    /// Creates the generator with a custom template engine.
    /// </summary>
    public ApplicationCommandGenerator(
        TemplateEngine templateEngine)
    {
        ArgumentNullException.ThrowIfNull(templateEngine);

        _templateEngine = templateEngine;
    }

    /// <inheritdoc />
    public override string Name => "application-command";

    /// <inheritdoc />
    protected override Task ValidateContextAsync(
        IGeneratorContext context,
        CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(context);

        cancellationToken.ThrowIfCancellationRequested();

        ValidateRequiredToken(
            context,
            "Namespace");

        var commandName = ValidateRequiredToken(
            context,
            "CommandName");

        EnsureValidIdentifier(
            commandName,
            "CommandName");

        var outputPath = GetOptionalToken(
            context,
            "OutputPath",
            DefaultOutputPath);

        EnsureSafeRelativePath(outputPath);

        return base.ValidateContextAsync(
            context,
            cancellationToken);
    }

    /// <inheritdoc />
    protected override async Task GenerateCoreAsync(
        IGeneratorContext context,
        GeneratorResult result,
        CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(context);
        ArgumentNullException.ThrowIfNull(result);

        cancellationToken.ThrowIfCancellationRequested();

        var namespaceName =
            GetRequiredToken(
                context,
                "Namespace");

        var commandName =
            GetRequiredToken(
                context,
                "CommandName");

        var outputPath =
            GetOptionalToken(
                context,
                "OutputPath",
                DefaultOutputPath);

        var tokens =
            new Dictionary<string, string>(
                StringComparer.OrdinalIgnoreCase)
            {
                ["Namespace"] = namespaceName,
                ["CommandName"] = commandName
            };

        var renderRequest =
            new TemplateRenderRequest
            {
                TemplateName = TemplateName,
                Tokens = tokens,
                RejectUnusedTokens = true,
                RejectUnresolvedPlaceholders = true
            };

        var renderResult =
            await _templateEngine.RenderAsync(
                renderRequest,
                cancellationToken);

        var relativePath = Path.Combine(
            outputPath,
            $"{commandName}.cs");

        AddArtifact(
            result,
            relativePath,
            renderResult.Content);
    }

    private static TemplateEngine CreateDefaultTemplateEngine()
    {
        var provider =
            new EmbeddedTemplateProvider(
                DefaultTemplates);

        var tokenReplacer =
            new TokenReplacer();

        return new TemplateEngine(
            provider,
            tokenReplacer);
    }

    private static string ValidateRequiredToken(
        IGeneratorContext context,
        string tokenName)
    {
        var value =
            GetRequiredToken(
                context,
                tokenName);

        if (string.IsNullOrWhiteSpace(value))
        {
            throw new GeneratorException(
                $"Required generator token '{tokenName}' cannot be empty.");
        }

        return value;
    }

    private static string GetRequiredToken(
        IGeneratorContext context,
        string tokenName)
    {
        ArgumentNullException.ThrowIfNull(context);
        ArgumentException.ThrowIfNullOrWhiteSpace(tokenName);

        if (!context.Tokens.TryGetValue(
                tokenName,
                out var value) ||
            string.IsNullOrWhiteSpace(value))
        {
            throw new GeneratorException(
                $"Required generator token '{tokenName}' was not provided.");
        }

        return value.Trim();
    }

    private static string GetOptionalToken(
        IGeneratorContext context,
        string tokenName,
        string defaultValue)
    {
        ArgumentNullException.ThrowIfNull(context);
        ArgumentException.ThrowIfNullOrWhiteSpace(tokenName);
        ArgumentNullException.ThrowIfNull(defaultValue);

        if (!context.Tokens.TryGetValue(
                tokenName,
                out var value) ||
            string.IsNullOrWhiteSpace(value))
        {
            return defaultValue;
        }

        return value.Trim();
    }

    private static void EnsureValidIdentifier(
        string value,
        string tokenName)
    {
        if (!CSharpIdentifierPattern().IsMatch(value))
        {
            throw new GeneratorException(
                $"Generator token '{tokenName}' must be a valid " +
                $"C# identifier. Received '{value}'.");
        }
    }

    private static void EnsureSafeRelativePath(
        string relativePath)
    {
        if (string.IsNullOrWhiteSpace(relativePath))
        {
            throw new GeneratorException(
                "OutputPath cannot be empty.");
        }

        if (Path.IsPathRooted(relativePath))
        {
            throw new GeneratorException(
                "OutputPath must be relative.");
        }

        var segments = relativePath.Split(
            [
                Path.DirectorySeparatorChar,
                Path.AltDirectorySeparatorChar
            ],
            StringSplitOptions.RemoveEmptyEntries);

        if (segments.Any(segment => segment == ".."))
        {
            throw new GeneratorException(
                "OutputPath cannot traverse parent directories.");
        }

        if (relativePath.IndexOfAny(
                Path.GetInvalidPathChars()) >= 0)
        {
            throw new GeneratorException(
                "OutputPath contains invalid path characters.");
        }
    }

    [GeneratedRegex(
        @"^[A-Za-z_][A-Za-z0-9_]*$",
        RegexOptions.CultureInvariant)]
    private static partial Regex CSharpIdentifierPattern();
}
