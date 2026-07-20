using Kyvara.Builder.Generators.Domain.Core;
using Kyvara.Builder.Generators.Framework.Abstractions;
using Kyvara.Builder.Generators.Framework.Models;
using Kyvara.Builder.Generators.Framework.Templates;

namespace Kyvara.Builder.Generators.Domain.ValueObjects;

/// <summary>
/// Generates a strongly typed domain value object.
/// </summary>
public sealed class DomainValueObjectGenerator : DomainGeneratorBase
{
    private const string TemplateName = "domain/value-object.cs";

    private const string DefaultUnderlyingType = "string";

    private const string DefaultOutputPath = "ValueObjects";

    private static readonly IReadOnlyDictionary<string, string>
        DefaultTemplates =
            new Dictionary<string, string>(
                StringComparer.OrdinalIgnoreCase)
            {
                [TemplateName] =
                    """
                    namespace {{Namespace}};

                    /// <summary>
                    /// Represents the {{ValueObjectName}} value object.
                    /// </summary>
                    public sealed class {{ValueObjectName}}
                        : IEquatable<{{ValueObjectName}}>
                    {
                        public {{ValueObjectName}}(
                            {{UnderlyingType}} value)
                        {
                            Value = value;
                        }

                        public {{UnderlyingType}} Value { get; }

                        public bool Equals(
                            {{ValueObjectName}}? other)
                        {
                            return other is not null &&
                                EqualityComparer<{{UnderlyingType}}>
                                    .Default
                                    .Equals(
                                        Value,
                                        other.Value);
                        }

                        public override bool Equals(object? obj)
                        {
                            return obj is {{ValueObjectName}} other &&
                                Equals(other);
                        }

                        public override int GetHashCode()
                        {
                            return EqualityComparer<{{UnderlyingType}}>
                                .Default
                                .GetHashCode(Value);
                        }

                        public override string ToString()
                        {
                            return Value is null
                                ? string.Empty
                                : Value.ToString() ?? string.Empty;
                        }
                    }
                    """
            };

    private readonly TemplateEngine _templateEngine;

    /// <summary>
    /// Creates the generator with its built-in value object template.
    /// </summary>
    public DomainValueObjectGenerator()
        : this(CreateTemplateEngine(DefaultTemplates))
    {
    }

    /// <summary>
    /// Creates the generator with a custom template engine.
    /// </summary>
    public DomainValueObjectGenerator(
        TemplateEngine templateEngine)
    {
        ArgumentNullException.ThrowIfNull(templateEngine);

        _templateEngine = templateEngine;
    }

    /// <inheritdoc />
    public override string Name => "domain-value-object";

    /// <inheritdoc />
    protected override async Task ValidateContextAsync(
        IGeneratorContext context,
        CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(context);

        cancellationToken.ThrowIfCancellationRequested();

        await base.ValidateContextAsync(
            context,
            cancellationToken);

        var namespaceName =
            GetRequiredToken(
                context,
                "Namespace");

        EnsureValidNamespace(
            namespaceName,
            "Namespace");

        var valueObjectName =
            GetRequiredToken(
                context,
                "ValueObjectName");

        EnsureValidIdentifier(
            valueObjectName,
            "ValueObjectName");

        var underlyingType =
            GetOptionalToken(
                context,
                "UnderlyingType",
                DefaultUnderlyingType);

        EnsureValidTypeName(
            underlyingType,
            "UnderlyingType");

        var outputPath =
            GetOptionalToken(
                context,
                "OutputPath",
                DefaultOutputPath);

        EnsureSafeRelativePath(
            outputPath,
            "OutputPath");
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
            EnsureValidNamespace(
                GetRequiredToken(
                    context,
                    "Namespace"),
                "Namespace");

        var valueObjectName =
            EnsureValidIdentifier(
                GetRequiredToken(
                    context,
                    "ValueObjectName"),
                "ValueObjectName");

        var underlyingType =
            EnsureValidTypeName(
                GetOptionalToken(
                    context,
                    "UnderlyingType",
                    DefaultUnderlyingType),
                "UnderlyingType");

        var outputPath =
            EnsureSafeRelativePath(
                GetOptionalToken(
                    context,
                    "OutputPath",
                    DefaultOutputPath),
                "OutputPath");

        var templateTokens =
            CreateTemplateTokens(
                ("Namespace", namespaceName),
                ("ValueObjectName", valueObjectName),
                ("UnderlyingType", underlyingType));

        var content =
            await RenderTemplateAsync(
                _templateEngine,
                TemplateName,
                templateTokens,
                cancellationToken);

        var relativePath =
            CreateArtifactPath(
                outputPath,
                $"{valueObjectName}.cs");

        AddDomainArtifact(
            result,
            relativePath,
            content);

        result.AddMessage(
            $"Generated value object '{valueObjectName}'.");
    }
}
