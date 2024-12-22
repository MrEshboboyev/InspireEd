using InspireEd.Infrastructure;
using Scrutor;

namespace InspireEd.App.Configurations;

public class InfrastructureServiceInstaller : IServiceInstaller
{
    public void Install(IServiceCollection services, IConfiguration configuration)
    {
        services
              .Scan(
                  selector => selector
                      .FromAssemblies(
                         AssemblyReference.Assembly,
                         InspireEd.Persistence.AssemblyReference.Assembly)
                      .AddClasses(false)
                      .UsingRegistrationStrategy(RegistrationStrategy.Skip)
                      .AsMatchingInterface()
                      .WithScopedLifetime());
    }
}