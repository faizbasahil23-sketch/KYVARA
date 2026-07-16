namespace Kyvara.Builder.Generators.Domain;

public sealed class SpecificationGenerator
{
    public string Generate()
    {
        return """
namespace {{Namespace}}.Specifications;

using System.Linq.Expressions;

public abstract class Specification<T>
{
    public abstract Expression<Func<T,bool>> ToExpression();

    public bool IsSatisfiedBy(T entity)
    {
        return ToExpression().Compile()(entity);
    }

    public Specification<T> And(Specification<T> other)
    {
        return new AndSpecification<T>(this,other);
    }

    public Specification<T> Or(Specification<T> other)
    {
        return new OrSpecification<T>(this,other);
    }
}
""";
    }
}
