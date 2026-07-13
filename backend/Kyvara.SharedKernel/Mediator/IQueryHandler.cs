namespace Kyvara.SharedKernel.Mediator;

public interface IQueryHandler<TQuery,TResult>
    where TQuery : IQuery<TResult>
{
    Task<TResult> Handle(
        TQuery query,
        CancellationToken cancellationToken);
}
