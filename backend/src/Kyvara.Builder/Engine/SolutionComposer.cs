namespace Kyvara.Builder.Engine;

public sealed class SolutionComposer
{
    private readonly SolutionBuilder _solutionBuilder;
    private readonly ProjectReferenceResolver _resolver;

    public SolutionComposer(
        SolutionBuilder solutionBuilder,
        ProjectReferenceResolver resolver)
    {
        _solutionBuilder = solutionBuilder;
        _resolver = resolver;
    }

    public EnterpriseSolution Compose()
    {
        var solution = new EnterpriseSolution(
            _solutionBuilder.SolutionName);

        foreach (var project in _solutionBuilder.GetProjects())
        {
            solution.AddProject(project);
        }

        foreach (var project in _solutionBuilder.GetTestProjects())
        {
            solution.AddProject(project);
        }

        _resolver.ConfigureDefaultEnterpriseReferences(
            _solutionBuilder.SolutionName);

        foreach (var pair in _resolver.GetAll())
        {
            foreach (var reference in pair.Value)
            {
                solution.AddReference(
                    pair.Key,
                    reference);
            }
        }

        return solution;
    }
}
