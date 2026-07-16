namespace Kyvara.Builder.Generators.Application;

public sealed class AutoMapperProfileGenerator
{
    public string Generate()
    {
        return """
using AutoMapper;

namespace {{Namespace}}.Mappings;

public sealed class ApplicationMappingProfile : Profile
{
    public ApplicationMappingProfile()
    {
        CreateMap<{{Entity}}, {{Entity}}Dto>();

        CreateMap<Create{{Entity}}Command, {{Entity}}>();
    }
}
""";
    }
}
