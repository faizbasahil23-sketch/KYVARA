namespace Kyvara.Builder.Commands;

public interface ICommand
{
    string Name { get; }

    Task ExecuteAsync(string[] args);
}
