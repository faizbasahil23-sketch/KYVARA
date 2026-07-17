namespace Kyvara.Builder.Generators.Framework.Templates;

public sealed class TemplateException : Exception
{
    public TemplateException(string message)
        : base(message)
    {
    }

    public TemplateException(
        string message,
        Exception innerException)
        : base(message, innerException)
    {
    }
}
