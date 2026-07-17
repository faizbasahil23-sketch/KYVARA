namespace Kyvara.Builder.Generators.Framework.Models;

public sealed class GeneratorResult
{
    private readonly List<GeneratorArtifact> _artifacts = [];
    private readonly List<string> _messages = [];
    private readonly List<string> _errors = [];

    public bool Success => _errors.Count == 0;

    public IReadOnlyList<GeneratorArtifact> Artifacts => _artifacts;

    public IReadOnlyList<string> Messages => _messages;

    public IReadOnlyList<string> Errors => _errors;

    public void AddArtifact(GeneratorArtifact artifact)
    {
        ArgumentNullException.ThrowIfNull(artifact);
        _artifacts.Add(artifact);
    }

    public void AddMessage(string message)
    {
        if (!string.IsNullOrWhiteSpace(message))
            _messages.Add(message);
    }

    public void AddError(string error)
    {
        if (!string.IsNullOrWhiteSpace(error))
            _errors.Add(error);
    }
}
