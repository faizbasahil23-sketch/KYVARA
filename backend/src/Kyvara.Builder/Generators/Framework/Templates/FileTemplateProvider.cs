using System.Collections.Concurrent;
using Kyvara.Builder.Generators.Framework.Abstractions;
using Kyvara.Builder.Generators.Framework.Templates.Validation;

namespace Kyvara.Builder.Generators.Framework.Templates;

public sealed class FileTemplateProvider : ITemplateProvider
{
    private readonly string _templateRoot;
    private readonly bool _enableCache;

    private readonly ConcurrentDictionary<string, string> _cache =
        new(StringComparer.OrdinalIgnoreCase);

    public FileTemplateProvider(
        string templateRoot,
        bool enableCache = true)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(templateRoot);

        _templateRoot = Path.GetFullPath(templateRoot);
        _enableCache = enableCache;
    }

    public string TemplateRoot => _templateRoot;

    public int CachedTemplateCount => _cache.Count;

    public bool Exists(string templateName)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(templateName);

        var path = TemplatePathGuard.ResolveSafePath(
            _templateRoot,
            templateName);

        return File.Exists(path);
    }

    public async Task<string> GetTemplateAsync(
        string templateName,
        CancellationToken cancellationToken = default)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(templateName);

        cancellationToken.ThrowIfCancellationRequested();

        var path = TemplatePathGuard.ResolveSafePath(
            _templateRoot,
            templateName);

        if (_enableCache &&
            _cache.TryGetValue(path, out var cached))
        {
            return cached;
        }

        if (!File.Exists(path))
        {
            throw new TemplateException(
                $"Template '{templateName}' was not found under " +
                $"'{_templateRoot}'.");
        }

        try
        {
            var content = await File.ReadAllTextAsync(
                path,
                cancellationToken);

            if (_enableCache)
            {
                _cache[path] = content;
            }

            return content;
        }
        catch (OperationCanceledException)
        {
            throw;
        }
        catch (Exception exception)
        {
            throw new TemplateException(
                $"Unable to read template '{templateName}'.",
                exception);
        }
    }

    public bool RemoveFromCache(string templateName)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(templateName);

        var path = TemplatePathGuard.ResolveSafePath(
            _templateRoot,
            templateName);

        return _cache.TryRemove(path, out _);
    }

    public void ClearCache()
    {
        _cache.Clear();
    }
}
