using Kyvara.Builder.Generators.Domain.Core;
using Kyvara.Builder.Generators.Framework.Abstractions;
using Kyvara.Builder.Generators.Framework.Models;
using Kyvara.Builder.Generators.Framework.Templates;

namespace Kyvara.Builder.Generators.Domain.Events;

/// <summary>
/// Generates a domain event record.
/// </summary>
public sealed class DomainEventGenerator : DomainGeneratorBase
{
    private const string TemplateName = "domain/event.cs";

    private const string DefaultOutputPath = "Events";

    private static readonly IReadOnlyDictionary<string, string>
        DefaultTemplates =
            new Dictionary<string, string>(
                StringComparer.OrdinalIgnoreCase)
            {
                [TemplateName] =
                    """
                    namespace {{Namespace}};

                    /// <summary>
                    /// Represents the {{EventName}} domain event.
                    /// </summary>
                    public sealed record {{EventName}}
                    {
                        private {{EventName}}(
                            DateTime occurredOnUtc)
                        {
                            OccurredOnUtc = occurredOnUtc;
                        }

                        public DateTime OccurredOnUtc { get; }

                        public static {{EventName}} Create()
                        {
                            return new {{EventName}}(
                                DateTime.UtcNow);
                        }
                    }
                    """
            };

    private readonly TemplateEngine _templateEngine;

    /// <summary>
    /// Creates the generator with its built-in event template.
    /// </summary>
    public DomainEventGenerator()
        : this(CreateTemplateEngine(DefaultTemplates))
    {
    }

    /// <summary>
    /// Creates the generator with a custom template engine.
    /// </summary>
    public DomainEventGenerator(
        TemplateEngine templateEngine)
    {
        ArgumentNullException.ThrowIfNull(templateEngine);

        _templateEngine = templateEngine;
    }

    /// <inheritdoc />
    public override string Name => "domain-event";

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

        var eventName =
            GetRequiredToken(
                context,
                "EventName");

        EnsureValidIdentifier(
            eventName,
            "EventName");

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

        var eventName =
            EnsureValidIdentifier(
                GetRequiredToken(
                    context,
                    "EventName"),
                "EventName");

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
                ("EventName", eventName));

        var content =
            await RenderTemplateAsync(
                _templateEngine,
                TemplateName,
                templateTokens,
                cancellationToken);

        var relativePath =
            CreateArtifactPath(
                outputPath,
                $"{eventName}.cs");

        AddDomainArtifact(
            result,
            relativePath,
            content);

        result.AddMessage(
            $"Generated domain event '{eventName}'.");
    }
}
