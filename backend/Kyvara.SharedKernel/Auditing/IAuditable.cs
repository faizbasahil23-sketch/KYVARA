namespace Kyvara.SharedKernel.Auditing;

public interface IAuditable
{
    DateTime CreatedAt { get; }

    DateTime? UpdatedAt { get; }
}
