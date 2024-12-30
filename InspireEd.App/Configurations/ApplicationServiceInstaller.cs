using FluentValidation;
using InspireEd.Application;
using InspireEd.Application.Behaviors;
using InspireEd.Application.Users.Services;
using InspireEd.Infrastructure.Idempotence;
using MediatR;

namespace InspireEd.App.Configurations;

/// <summary>
/// Installs application services and configuration.
/// </summary>
public class ApplicationServiceInstaller : IServiceInstaller
{
    /// <summary>
    /// Configures the application services.
    /// </summary>
    /// <param name="services">The collection of services to configure.</param>
    /// <param name="configuration">The application configuration.</param>
    public void Install(IServiceCollection services, IConfiguration configuration)
    {
        // Add MediatR services for handling commands and queries
        services.AddMediatR(AssemblyReference.Assembly);

        // Add validation behavior to the pipeline
        services.AddScoped(typeof(IPipelineBehavior<,>), typeof(ValidationPipelineBehavior<,>));

        // Add logging behavior to the pipeline
        services.AddScoped(typeof(IPipelineBehavior<,>), typeof(LoggingPipelineBehavior<,>));

        // Decorate notification handlers with idempotent behavior
        services.Decorate(typeof(INotificationHandler<>), typeof(IdempotentDomainEventHandler<>));

        // Add FluentValidation validators from the specified assembly
        services.AddValidatorsFromAssembly(
            AssemblyReference.Assembly,
            includeInternalTypes: true);
        
        // Add Services lifetimes
        services.AddScoped<IUserCreationService, UserCreationService>();

    }
}