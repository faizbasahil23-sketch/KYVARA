using Kyvara.Builder.Generators.Domain.Core;
using Kyvara.Builder.Generators.Framework.Abstractions;
using Kyvara.Builder.Generators.Framework.Models;
using Kyvara.Builder.Generators.Framework.Templates;

namespace Kyvara.Builder.Generators.Domain.Aggregates;

/// <summary>
/// Generates a domain aggregate root class.
/// </summary>
public sealed class DomainAggregateGenerator : DomainGeneratorBase
{
    private const string TemplateName = "domain/aggregate.cs";

    private const string DefaultIdType = "Guid";

    private const string DefaultOutputPath = "Aggregates";

    private static readonly IReadOnlyDictionary<string, string>
        DefaultTemplates =
            new Dictionary<string, string>(
                StringComparer.OrdinalIgnoreCase)
            {
                [TemplateName] =
                    """
                    namespace {{Namespace}};

                    /// <summary>
                    /// Represents the {{AggregateName}} aggregate root.
                    /// </summary>
                    public sealed class {{AggregateName}}
                    {
                        private {{AggregateName}}()
                        {
                        }

                        public {{AggregateName}}({{IdType}} id)
                        {
                            Id = id;
                        }

                        public {{IdType}} Id { get; private set; }
                    }
                    """
            };

    private readonly TemplateEngine _templateEngine;

    /// <summary>
    /// Creates the generator with its built-in aggregate template.
    /// </summary>
    public DomainAggregateGenerator()
        : this(CreateTemplateEngine(DefaultTemplates))
    {
    }

    /// <summary>
    /// Creates the generator with a custom template engine.
    /// </summary>
    public DomainAggregateGenerator(
        TemplateEngine templateEngine)
    {
        ArgumentNullException.ThrowIfNull(templateEngine);

        _templateEngine = templateEngine;
    }

    /// <inheritdoc />
    public override string Name => "domain-aggregate";

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
            GetRequiredToken(context, "Namespace");

        EnsureValidNamespace(
            namespaceName,
            "Namespace");

        var aggregateName =
            GetRequiredToken(context, "AggregateName");

        EnsureValidIdentifier(
            aggregateName,
            "AggregateName");

        var idType =
            GetOptionalToken(
                context,
                "IdType",
                DefaultIdType);

        EnsureValidTypeName(
            idType,
            "IdType");

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

        var aggregateName =
            EnsureValidIdentifier(
                GetRequiredToken(
                    context,
                    "AggregateName"),
                "AggregateName");

        var idType =
            EnsureValidTypeName(
                GetOptionalToken(
                    context,
                    "IdType",
                    DefaultIdType),
                "IdType");

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
                ("AggregateName", aggregateName),
                ("IdType", idType));

        var content =
            await RenderTemplateAsync(
                _templateEngine,
                TemplateName,
                templateTokens,
                cancellationToken);

        var relativePath =
            CreateArtifactPath(
                outputPath,
                $"{aggregateName}.cs");

        AddDomainArtifact(
            result,
            relativePath,
            content);

        result.AddMessage(
            $"Generated aggregate root '{aggregateName}'.");
    }
}
