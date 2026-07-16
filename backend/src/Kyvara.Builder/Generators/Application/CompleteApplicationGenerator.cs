namespace Kyvara.Builder.Generators.Application;

public sealed class CompleteApplicationGenerator
{
    private readonly CommandGenerator _command = new();
    private readonly QueryGenerator _query = new();
    private readonly CommandHandlerGenerator _commandHandler = new();
    private readonly QueryHandlerGenerator _queryHandler = new();
    private readonly DtoGenerator _dto = new();
    private readonly ValidatorGenerator _validator = new();
    private readonly AutoMapperProfileGenerator _mapper = new();
    private readonly PipelineBehaviorGenerator _pipeline = new();

    public Dictionary<string,string> Generate()
    {
        var files = new Dictionary<string,string>();

        files["Commands/CreateCommand.cs"] =
            _command.Generate();

        files["Queries/GetQuery.cs"] =
            _query.Generate();

        files["Handlers/CreateCommandHandler.cs"] =
            _commandHandler.Generate();

        files["Handlers/GetQueryHandler.cs"] =
            _queryHandler.Generate();

        files["Dtos/EntityDto.cs"] =
            _dto.Generate();

        files["Validators/CommandValidator.cs"] =
            _validator.Generate();

        files["Mappings/ApplicationMappingProfile.cs"] =
            _mapper.Generate();

        files["Behaviors/LoggingBehavior.cs"] =
            _pipeline.Generate();

        return files;
    }
}
