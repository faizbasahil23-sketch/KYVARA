using Microsoft.Extensions.DependencyInjection;
using Kyvara.SharedKernel.DependencyInjection;

namespace Kyvara.SharedKernel.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddKyvaraSharedKernel(
        this IServiceCollection services)
    {
        var installer = new SharedKernelInstaller();

        installer.Install(services);

        return services;
    }
}
