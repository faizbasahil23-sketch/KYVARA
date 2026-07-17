namespace Kyvara.Builder.Engine;

public sealed class TemplateEngine
{
    public string Render(
        string template,
        Dictionary<string,string> values)
    {
        var output = template;

        foreach(var pair in values)
        {
            output = output.Replace(
                "{{" + pair.Key + "}}",
                pair.Value);
        }

        return output;
    }
}
