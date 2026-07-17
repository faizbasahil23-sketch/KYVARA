namespace Kyvara.Builder.Engine;

public sealed record VerificationResult(
    bool Success,
    IReadOnlyList<string> Errors);

public sealed class SolutionVerifier
{
    public VerificationResult Verify(
        EnterpriseSolution solution)
    {
        var errors = new List<string>();

        if (solution is null)
        {
            errors.Add("Solution is null.");

            return new VerificationResult(
                false,
                errors);
        }

        if (string.IsNullOrWhiteSpace(solution.Name))
            errors.Add("Solution name is empty.");

        if (solution.Projects.Count == 0)
            errors.Add("Solution has no projects.");

        var duplicates =
            solution.Projects
                .GroupBy(x => x)
                .Where(g => g.Count() > 1);

        foreach (var duplicate in duplicates)
        {
            errors.Add(
                $"Duplicate project: {duplicate.Key}");
        }

        foreach (var project in solution.References)
        {
            if (!solution.Projects.Contains(project.Key))
            {
                errors.Add(
                    $"Missing project: {project.Key}");

                continue;
            }

            foreach (var reference in project.Value)
            {
                if (!solution.Projects.Contains(reference))
                {
                    errors.Add(
                        $"Project '{project.Key}' references missing project '{reference}'.");
                }
            }
        }

        return new VerificationResult(
            errors.Count == 0,
            errors);
    }
}
