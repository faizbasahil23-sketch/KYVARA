namespace Kyvara.Builder.Engine;

public sealed class EnterpriseScaffolding
{
    private readonly SolutionComposer _composer;
    private readonly ProjectCreator _creator;
    private readonly RestoreEngine _restore;
    private readonly BuildValidator _build;
    private readonly SolutionVerifier _verifier;

    public EnterpriseScaffolding(
        SolutionComposer composer,
        ProjectCreator creator,
        RestoreEngine restore,
        BuildValidator build,
        SolutionVerifier verifier)
    {
        _composer = composer;
        _creator = creator;
        _restore = restore;
        _build = build;
        _verifier = verifier;
    }

    public bool Generate(string outputDirectory)
    {
        var solution = _composer.Compose();

        var verification = _verifier.Verify(solution);

        if (!verification.Success)
        {
            foreach (var error in verification.Errors)
            {
                Console.WriteLine(error);
            }

            return false;
        }

        _creator.Create(solution, outputDirectory);

        if (!_restore.Restore(outputDirectory))
            return false;

        var build = _build.Validate(outputDirectory);

        if (!build.Success)
        {
            Console.WriteLine(build.Errors);
            return false;
        }

        Console.WriteLine("Enterprise scaffolding completed successfully.");

        return true;
    }
}
