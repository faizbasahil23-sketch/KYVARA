# KYVARA Generator Pipeline

The generator pipeline coordinates a complete generator execution.

## Execution stages

1. Validate the generator request.
2. Resolve the requested generator.
3. Create the generator context.
4. Execute the generator.
5. Write generated artifacts.
6. Return the generator result.

## Main components

- `GeneratorPipeline`
- `PipelineExecutionContext`
- `IPipelineStep`
- `ValidateRequestStep`
- `ResolveGeneratorStep`
- `CreateGeneratorContextStep`
- `ExecuteGeneratorStep`
- `WriteArtifactsStep`

## Error behavior

Pipeline errors are captured inside `GeneratorResult.Errors`.

A failed or cancelled pipeline returns a result whose `Success`
property is `false`.

## File-writing abstraction

The pipeline depends on `IGeneratedFileWriter`.

The concrete filesystem writer will be implemented in milestone
M014.0F. Tests can use an in-memory writer.
