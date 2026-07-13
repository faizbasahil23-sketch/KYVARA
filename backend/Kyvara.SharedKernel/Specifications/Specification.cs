using System.Linq.Expressions;

namespace Kyvara.SharedKernel.Specifications;

public abstract class Specification<T>
    : ISpecification<T>
{
    public Expression<Func<T,bool>>? Criteria { get; protected set; }

    public List<Expression<Func<T,object>>> Includes { get; }
        = new();

    protected void AddInclude(
        Expression<Func<T,object>> include)
    {
        Includes.Add(include);
    }
}
