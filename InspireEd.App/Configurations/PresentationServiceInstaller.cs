using InspireEd.Presentation;

namespace InspireEd.App.Configurations;

public class PresentationServiceInstaller : IServiceInstaller
{
    public void Install(IServiceCollection services, IConfiguration configuration)
    {
        services
            .AddControllers()
            .AddApplicationPart(AssemblyReference.Assembly);
        services.AddOpenApi();
    }
}