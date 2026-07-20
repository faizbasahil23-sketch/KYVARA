using Kyvara.Builder.Generators.Domain.Core;
using Kyvara.Builder.Generators.Framework.Abstractions;
using Kyvara.Builder.Generators.Framework.Models;
using Kyvara.Builder.Generators.Framework.Templates;

namespace Kyvara.Builder.Generators.Domain.Exceptions;

/// <summary>
/// Generates a domain-specific exception class.
/// </summary>
public sealed class DomainExceptionGenerator : DomainGeneratorBase
{
    private const string TemplateName = "domain/exception.cs";

    private const string DefaultBaseException = "Exception";

    private const string DefaultOutputPath = "Exceptions";

    private static readonly IReadOnlyDictionary<string, string>
        DefaultTemplates =
            new Dictionary<string, string>(
                StringComparer.OrdinalIgnoreCase)
            {
                [TemplateName] =
                    """
                    namespace {{Namespace}};

                    /// <summary>
                    /// Represents the {{ExceptionName}} domain exception.
                    /// </summary>
                    public sealed class {{ExceptionName}}
                        : {{BaseException}}
                    {
                        public {{ExceptionName}}()
                        {
                        }

                        public {{ExceptionName}}(
                            string message)
                            : base(message)
                        {
                        }

                        public {{ExceptionName}}(
                            string message,
                            Exception? innerException)
                            : base(
                                message,
                                innerException)
                        {
                        }
                    }
                    """
            };

    private readonly TemplateEngine _templateEngine;

    /// <summary>
    /// Creates the generator with its built-in exception template.
    /// </summary>
    public DomainExceptionGenerator()
        : this(CreateTemplateEngine(DefaultTemplates))
    {
    }

    /// <summary>
    /// Creates the generator with a custom template engine.
    /// </summary>
    public DomainExceptionGenerator(
        TemplateEngine templateEngine)
    {
        ArgumentNullException.ThrowIfNull(templateEngine);

        _templateEngine = templateEngine;
    }

    /// <inheritdoc />
    public override string Name => "domain-exception";

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

        var exceptionName =
            GetRequiredToken(
                context,
                "ExceptionName");

        EnsureValidIdentifier(
            exceptionName,
            "ExceptionName");

        var baseException =
            GetOptionalToken(
                context,
                "BaseException",
                DefaultBaseException);

        EnsureValidTypeName(
            baseException,
            "BaseException");

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

        var exceptionName =
            EnsureValidIdentifier(
                GetRequiredToken(
                    context,
                    "ExceptionName"),
                "ExceptionName");

        var baseException =
            EnsureValidTypeName(
                GetOptionalToken(
                    context,
                    "BaseException",
                    DefaultBaseException),
                "BaseException");

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
                ("ExceptionName", exceptionName),
                ("BaseException", baseException));

        var content =
            await RenderTemplateAsync(
                _templateEngine,
                TemplateName,
                templateTokens,
                cancellationToken);

        var relativePath =
            CreateArtifactPath(
                outputPath,
                $"{exceptionName}.cs");

        AddDomainArtifact(
            result,
            relativePath,
            content);

        result.AddMessage(
            $"Generated domain exception '{exceptionName}'.");
    }
}
