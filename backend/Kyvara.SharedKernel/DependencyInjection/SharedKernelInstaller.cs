using Microsoft.Extensions.DependencyInjection;
using Kyvara.SharedKernel.Events;

namespace Kyvara.SharedKernel.DependencyInjection;

public sealed class SharedKernelInstaller
    : IServiceInstaller
{
    public void Install(
        IServiceCollection services)
    {
        services.AddSingleton<IDomainEventDispatcher,DomainEventDispatcher>();
    }
}
