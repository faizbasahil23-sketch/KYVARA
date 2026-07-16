namespace Kyvara.Builder.Generators.Infrastructure;

public sealed class CompleteInfrastructureGenerator
{
    private readonly DbContextGenerator _dbContext = new();
    private readonly RepositoryImplementationGenerator _repository = new();
    private readonly UnitOfWorkGenerator _unitOfWork = new();
    private readonly EntityConfigurationGenerator _configuration = new();
    private readonly MigrationGenerator _migration = new();
    private readonly SeedDataGenerator _seed = new();
    private readonly InfrastructureDependencyInjectionGenerator _dependencyInjection = new();

    public Dictionary<string,string> Generate()
    {
        var files = new Dictionary<string,string>();

        files["Persistence/ApplicationDbContext.cs"] =
            _dbContext.Generate();

        files["Persistence/Repository.cs"] =
            _repository.Generate();

        files["Persistence/UnitOfWork.cs"] =
            _unitOfWork.Generate();

        files["Persistence/Configurations/EntityConfiguration.cs"] =
            _configuration.Generate();

        files["Persistence/MigrationRunner.cs"] =
            _migration.Generate();

        files["Persistence/SeedData.cs"] =
            _seed.Generate();

        files["DependencyInjection.cs"] =
            _dependencyInjection.Generate();

        return files;
    }
}
