namespace Kyvara.Builder.Generators.Application;

public sealed class MappingProfileGenerator
{
    public string Generate()
    {
        return """
using AutoMapper;

namespace {{Namespace}}.Mappings;

public sealed class {{Entity}}MappingProfile : Profile
{
    public {{Entity}}MappingProfile()
    {
        CreateMap<{{Entity}}, {{Entity}}Dto>();

        CreateMap<Create{{Entity}}Command, {{Entity}}>();

        CreateMap<Update{{Entity}}Command, {{Entity}}>();

        CreateMap<Create{{Entity}}Dto, {{Entity}}>();

        CreateMap<Update{{Entity}}Dto, {{Entity}}>();

        CreateMap<{{Entity}}, {{Entity}}SummaryDto>();

        CreateMap<{{Entity}}, {{Entity}}DetailDto>();
    }
}
""";
    }
}
