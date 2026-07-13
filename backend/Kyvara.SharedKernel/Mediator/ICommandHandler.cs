namespace Kyvara.SharedKernel.Mediator;

public interface ICommandHandler<TCommand>
    where TCommand : ICommand
{
    Task Handle(
        TCommand command,
        CancellationToken cancellationToken);
}
