using InspireEd.Infrastructure.Authentication;
using Microsoft.AspNetCore.Authorization;

namespace InspireEd.App.Configurations;

public class AuthorizationServiceInstaller : IServiceInstaller
{
    public void Install(IServiceCollection services, IConfiguration configuration)
    {
        services.AddAuthorization();
        services.AddSingleton<IAuthorizationHandler, PermissionAuthorizationHandler>();
        services.AddSingleton<IAuthorizationPolicyProvider, PermissionAuthorizationPolicyProvider>();
    }
}