# KYVARA Template Engine

The Template Engine renders source-code templates by replacing named placeholders.

## Placeholder format

Placeholders use this format:

{{TokenName}}

Example:

namespace {{Namespace}}.Entities;

public sealed class {{EntityName}}
{
}

## Main components

- TemplateEngine
- FileTemplateProvider
- EmbeddedTemplateProvider
- TokenReplacer
- TemplateValidator
- TemplatePlaceholderParser
- TemplatePathGuard
- TemplateRenderRequest
- TemplateRenderResult
- TemplateException

## Rendering pipeline

1. Load the requested template.
2. Parse placeholders.
3. Validate supplied tokens.
4. Replace placeholders.
5. Detect unresolved placeholders.
6. Return rendered content.

## Generator Framework integration

Generators render templates into GeneratorArtifact objects.
Artifacts are processed by the Generator Pipeline and Generated File Writer.
