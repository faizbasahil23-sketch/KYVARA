using Microsoft.Extensions.DependencyInjection;

namespace Kyvara.SharedKernel.DependencyInjection;

public interface IServiceInstaller
{
    void Install(
        IServiceCollection services);
}
