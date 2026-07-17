# KYVARA Generated File Writer

The generated file writer persists generator artifacts safely.

## Main components

- `GeneratedFileWriter`
- `GeneratedFileWriterOptions`
- `GeneratedFileWriteResult`
- `GeneratedFileWriteJournal`
- `GeneratedFilePathGuard`
- `GeneratedContentNormalizer`
- `GeneratedFileWriterFactory`

## Safety behavior

The writer rejects:

- absolute artifact paths;
- parent-directory traversal;
- paths outside the configured output directory;
- invalid file or directory names.

## Atomic writes

Content is first written to a temporary file in the destination
directory. The temporary file is then moved to the final destination.

This prevents partially written generated source files.

## Overwrite policy

When an output file already exists:

- overwrite disabled: writing fails;
- overwrite enabled: the file is replaced;
- backup enabled: the original file is copied before replacement;
- unchanged content: writing is skipped when configured.

## Dry-run mode

Dry-run mode validates the path and reports the intended operation
without creating directories or writing files.

## Writer profiles

Use `GeneratedFileWriterFactory` to create:

- default writer;
- safe-overwrite writer;
- dry-run writer.

## Pipeline integration

`GeneratedFileWriter` implements `IGeneratedFileWriter`, so it can be
passed directly to `GeneratorPipeline`.
