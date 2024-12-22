using FluentValidation;
using InspireEd.Application;
using InspireEd.Application.Behaviors;
using InspireEd.Infrastructure.Idempotence;
using MediatR;

namespace InspireEd.App.Configurations;

public class ApplicationServiceInstaller : IServiceInstaller
{
    public void Install(IServiceCollection services, IConfiguration configuration)
    {
        services.AddMediatR(AssemblyReference.Assembly);

        services.AddScoped(typeof(IPipelineBehavior<,>), typeof(ValidationPipelineBehavior<,>));

        services.AddScoped(typeof(IPipelineBehavior<,>), typeof(LoggingPipelineBehavior<,>));

        services.Decorate(typeof(INotificationHandler<>), typeof(IdempotentDomainEventHandler<>));

        services.AddValidatorsFromAssembly(
            AssemblyReference.Assembly,
            includeInternalTypes: true);
    }
}