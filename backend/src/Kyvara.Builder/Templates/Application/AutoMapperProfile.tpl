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
