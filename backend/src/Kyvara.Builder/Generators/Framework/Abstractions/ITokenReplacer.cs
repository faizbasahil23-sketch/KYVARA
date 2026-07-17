namespace Kyvara.Builder.Generators.Framework.Abstractions;

public interface ITokenReplacer
{
    string Replace(
        string template,
        IReadOnlyDictionary<string, string> tokens);
}
