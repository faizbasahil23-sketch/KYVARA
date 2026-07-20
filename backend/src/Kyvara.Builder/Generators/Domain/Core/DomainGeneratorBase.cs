using System.Text.RegularExpressions;
using Kyvara.Builder.Generators.Framework.Abstractions;
using Kyvara.Builder.Generators.Framework.Core;
using Kyvara.Builder.Generators.Framework.Templates;

namespace Kyvara.Builder.Generators.Domain.Core;

/// <summary>
/// Provides shared validation, token access, path handling, and template
/// rendering behavior for domain artifact generators.
/// </summary>
public abstract class DomainGeneratorBase : GeneratorBase
{
    private static readonly Regex IdentifierPattern = new(
        @"^[_\p{L}][_\p{L}\p{Nd}]*$",
        RegexOptions.Compiled |
        RegexOptions.CultureInvariant);

    private static readonly Regex TypeNamePattern = new(
        @"^[_\p{L}][_\p{L}\p{Nd}]*" +
        @"(?:\s*<\s*[_\p{L}][_\p{L}\p{Nd}.<>,?\[\]\s]*\s*>)?" +
        @"(?:\[\])?\??$",
        RegexOptions.Compiled |
        RegexOptions.CultureInvariant);

    /// <summary>
    /// Gets a required token and rejects missing or whitespace-only values.
    /// </summary>
    protected static string GetRequiredToken(
        IGeneratorContext context,
        string tokenName)
    {
        ArgumentNullException.ThrowIfNull(context);
        ArgumentException.ThrowIfNullOrWhiteSpace(tokenName);

        if (!context.Tokens.TryGetValue(tokenName, out var value) ||
            string.IsNullOrWhiteSpace(value))
        {
            throw new GeneratorException(
                $"Required generator token '{tokenName}' was not provided.");
        }

        return value.Trim();
    }

    /// <summary>
    /// Gets an optional token, returning the supplied default when the token
    /// does not contain a meaningful value.
    /// </summary>
    protected static string GetOptionalToken(
        IGeneratorContext context,
        string tokenName,
        string defaultValue)
    {
        ArgumentNullException.ThrowIfNull(context);
        ArgumentException.ThrowIfNullOrWhiteSpace(tokenName);
        ArgumentNullException.ThrowIfNull(defaultValue);

        if (!context.Tokens.TryGetValue(tokenName, out var value) ||
            string.IsNullOrWhiteSpace(value))
        {
            return defaultValue;
        }

        return value.Trim();
    }

    /// <summary>
    /// Determines whether a non-empty token is available.
    /// </summary>
    protected static bool HasToken(
        IGeneratorContext context,
        string tokenName)
    {
        ArgumentNullException.ThrowIfNull(context);
        ArgumentException.ThrowIfNullOrWhiteSpace(tokenName);

        return context.Tokens.TryGetValue(tokenName, out var value) &&
            !string.IsNullOrWhiteSpace(value);
    }

    /// <summary>
    /// Validates a required token without returning its value.
    /// </summary>
    protected static void ValidateRequiredToken(
        IGeneratorContext context,
        string tokenName)
    {
        _ = GetRequiredToken(context, tokenName);
    }

    /// <summary>
    /// Ensures that a value is a valid C# identifier.
    /// </summary>
    protected static string EnsureValidIdentifier(
        string value,
        string tokenName)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(value);
        ArgumentException.ThrowIfNullOrWhiteSpace(tokenName);

        var normalized = value.Trim();

        if (!IdentifierPattern.IsMatch(normalized))
        {
            throw new GeneratorException(
                $"Generator token '{tokenName}' must be a valid " +
                $"C# identifier. Received: '{value}'.");
        }

        return normalized;
    }

    /// <summary>
    /// Ensures that a value is a supported C# type name.
    /// </summary>
    protected static string EnsureValidTypeName(
        string value,
        string tokenName)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(value);
        ArgumentException.ThrowIfNullOrWhiteSpace(tokenName);

        var normalized = value.Trim();

        if (!TypeNamePattern.IsMatch(normalized))
        {
            throw new GeneratorException(
                $"Generator token '{tokenName}' must be a valid " +
                $"C# type name. Received: '{value}'.");
        }

        return normalized;
    }

    /// <summary>
    /// Ensures that a value is a valid dotted C# namespace.
    /// </summary>
    protected static string EnsureValidNamespace(
        string value,
        string tokenName = "Namespace")
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(value);
        ArgumentException.ThrowIfNullOrWhiteSpace(tokenName);

        var normalized = value.Trim();

        var namespaceSegments = normalized.Split(
            '.',
            StringSplitOptions.RemoveEmptyEntries |
            StringSplitOptions.TrimEntries);

        if (namespaceSegments.Length == 0 ||
            namespaceSegments.Any(
                segment => !IdentifierPattern.IsMatch(segment)))
        {
            throw new GeneratorException(
                $"Generator token '{tokenName}' must be a valid " +
                $"C# namespace. Received: '{value}'.");
        }

        return string.Join('.', namespaceSegments);
    }

    /// <summary>
    /// Ensures that an artifact output path is relative and cannot escape the
    /// configured generator output directory.
    /// </summary>
    protected static string EnsureSafeRelativePath(
        string value,
        string tokenName = "OutputPath")
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(value);
        ArgumentException.ThrowIfNullOrWhiteSpace(tokenName);

        if (Path.IsPathRooted(value))
        {
            throw new GeneratorException(
                $"Generator token '{tokenName}' must contain a " +
                $"relative path.");
        }

        var normalized = value
            .Replace(
                Path.AltDirectorySeparatorChar,
                Path.DirectorySeparatorChar)
            .Trim()
            .TrimStart(Path.DirectorySeparatorChar);

        var segments = normalized.Split(
            Path.DirectorySeparatorChar,
            StringSplitOptions.RemoveEmptyEntries);

        if (segments.Length == 0)
        {
            throw new GeneratorException(
                $"Generator token '{tokenName}' cannot be empty.");
        }

        if (segments.Any(segment => segment is "." or ".."))
        {
            throw new GeneratorException(
                $"Generator token '{tokenName}' cannot contain " +
                $"directory traversal segments.");
        }

        if (segments.Any(
                segment =>
                    segment.IndexOfAny(
                        Path.GetInvalidFileNameChars()) >= 0))
        {
            throw new GeneratorException(
                $"Generator token '{tokenName}' contains invalid " +
                $"path characters.");
        }

        return Path.Combine(segments);
    }

    /// <summary>
    /// Combines a validated relative output directory with a generated file
    /// name.
    /// </summary>
    protected static string CreateArtifactPath(
        string outputPath,
        string fileName)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(outputPath);
        ArgumentException.ThrowIfNullOrWhiteSpace(fileName);

        var safeOutputPath = EnsureSafeRelativePath(outputPath);

        if (Path.IsPathRooted(fileName) ||
            fileName.Contains(Path.DirectorySeparatorChar) ||
            fileName.Contains(Path.AltDirectorySeparatorChar) ||
            fileName is "." or ".." ||
            fileName.IndexOfAny(Path.GetInvalidFileNameChars()) >= 0)
        {
            throw new GeneratorException(
                $"Generated artifact file name '{fileName}' is invalid.");
        }

        return Path.Combine(
            safeOutputPath,
            fileName);
    }

    /// <summary>
    /// Creates a template engine backed by an in-memory template collection.
    /// </summary>
    protected static TemplateEngine CreateTemplateEngine(
        IReadOnlyDictionary<string, string> templates)
    {
        ArgumentNullException.ThrowIfNull(templates);

        return new TemplateEngine(
            new EmbeddedTemplateProvider(templates),
            new TokenReplacer());
    }

    /// <summary>
    /// Renders a template using strict placeholder validation.
    /// </summary>
    protected static async Task<string> RenderTemplateAsync(
        TemplateEngine templateEngine,
        string templateName,
        IReadOnlyDictionary<string, string> tokens,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(templateEngine);
        ArgumentException.ThrowIfNullOrWhiteSpace(templateName);
        ArgumentNullException.ThrowIfNull(tokens);

        cancellationToken.ThrowIfCancellationRequested();

        var request = new TemplateRenderRequest
        {
            TemplateName = templateName,
            Tokens = tokens,
            RejectUnusedTokens = true,
            RejectUnresolvedPlaceholders = true
        };

        var renderResult = await templateEngine.RenderAsync(
            request,
            cancellationToken);

        return renderResult.Content;
    }

    /// <summary>
    /// Creates a case-insensitive token dictionary suitable for template
    /// rendering.
    /// </summary>
    protected static Dictionary<string, string> CreateTemplateTokens(
        params (string Name, string Value)[] tokens)
    {
        ArgumentNullException.ThrowIfNull(tokens);

        var result = new Dictionary<string, string>(
            StringComparer.OrdinalIgnoreCase);

        foreach (var token in tokens)
        {
            ArgumentException.ThrowIfNullOrWhiteSpace(token.Name);
            ArgumentNullException.ThrowIfNull(token.Value);

            if (!result.TryAdd(token.Name, token.Value))
            {
                throw new GeneratorException(
                    $"Template token '{token.Name}' was added more than once.");
            }
        }

        return result;
    }

    /// <summary>
    /// Adds a rendered domain artifact to a generator result.
    /// </summary>
    protected static void AddDomainArtifact(
        Framework.Models.GeneratorResult result,
        string relativePath,
        string content)
    {
        ArgumentNullException.ThrowIfNull(result);

        var safeRelativePath =
            EnsureSafeRelativePath(relativePath, nameof(relativePath));

        AddArtifact(
            result,
            safeRelativePath,
            content);
    }

    /// <inheritdoc />
    protected override async Task ValidateContextAsync(
        IGeneratorContext context,
        CancellationToken cancellationToken)
    {
        await base.ValidateContextAsync(
            context,
            cancellationToken);

        cancellationToken.ThrowIfCancellationRequested();

        ArgumentNullException.ThrowIfNull(context.Tokens);
    }
}
