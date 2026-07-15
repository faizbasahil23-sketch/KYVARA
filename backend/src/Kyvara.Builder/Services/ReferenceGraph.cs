using Kyvara.Builder.Models;

namespace Kyvara.Builder.Services;

public sealed class ReferenceGraph
{
    public List<ProjectReference> Build(string module)
    {
        return
        [
            new()
            {
                Project=$"{module}.Api",
                References=
                {
                    $"{module}.Application",
                    $"{module}.Infrastructure",
                    $"{module}.Contracts"
                }
            },

            new()
            {
                Project=$"{module}.Application",
                References=
                {
                    $"{module}.Domain",
                    $"{module}.Contracts"
                }
            },

            new()
            {
                Project=$"{module}.Infrastructure",
                References=
                {
                    $"{module}.Application",
                    $"{module}.Domain"
                }
            },

            new()
            {
                Project=$"{module}.UnitTests",
                References=
                {
                    $"{module}.Application",
                    $"{module}.Domain"
                }
            },

            new()
            {
                Project=$"{module}.IntegrationTests",
                References=
                {
                    $"{module}.Api"
                }
            },

            new()
            {
                Project=$"{module}.ArchitectureTests",
                References=
                {
                    $"{module}.Domain",
                    $"{module}.Application",
                    $"{module}.Infrastructure"
                }
            }
        ];
    }
}
