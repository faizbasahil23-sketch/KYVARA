using System.Text;

namespace Kyvara.Builder.Generators.Domain;

public sealed class CompleteDomainGenerator
{
    private readonly EntityGenerator _entity = new();
    private readonly ValueObjectGenerator _valueObject = new();
    private readonly AggregateGenerator _aggregate = new();
    private readonly DomainEventGenerator _event = new();
    private readonly RepositoryGenerator _repository = new();
    private readonly SpecificationGenerator _specification = new();
    private readonly DomainServiceGenerator _service = new();
    private readonly BusinessRuleGenerator _rule = new();
    private readonly DomainPolicyGenerator _policy = new();
    private readonly DomainExceptionGenerator _exception = new();

    public Dictionary<string,string> Generate(
        string @namespace,
        string entityName)
    {
        var files = new Dictionary<string,string>();

        files["Entities/" + entityName + ".cs"] =
            _entity.Generate();

        files["ValueObjects/" + entityName + "Value.cs"] =
            _valueObject.Generate();

        files["Aggregates/AggregateRoot.cs"] =
            _aggregate.Generate();

        files["Events/DomainEvent.cs"] =
            _event.Generate();

        files["Repositories/IRepository.cs"] =
            _repository.Generate();

        files["Specifications/Specification.cs"] =
            _specification.Generate();

        files["Services/DomainService.cs"] =
            _service.Generate();

        files["Rules/IBusinessRule.cs"] =
            _rule.Generate();

        files["Policies/DomainPolicy.cs"] =
            _policy.Generate();

        files["Exceptions/DomainException.cs"] =
            _exception.Generate();

        return files;
    }
}
