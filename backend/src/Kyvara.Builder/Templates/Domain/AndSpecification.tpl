namespace {{Namespace}}.Specifications;

using System.Linq.Expressions;

public sealed class AndSpecification<T> : Specification<T>
{
    private readonly Specification<T> _left;
    private readonly Specification<T> _right;

    public AndSpecification(
        Specification<T> left,
        Specification<T> right)
    {
        _left = left;
        _right = right;
    }

    public override Expression<Func<T,bool>> ToExpression()
    {
        return entity =>
            _left.IsSatisfiedBy(entity)
            && _right.IsSatisfiedBy(entity);
    }
}
