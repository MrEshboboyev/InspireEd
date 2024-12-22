using InspireEd.App.OptionsSetup;
using Microsoft.AspNetCore.Authentication.JwtBearer;

namespace InspireEd.App.Configurations;

public class AuthenticationServiceInstaller : IServiceInstaller
{
    public void Install(IServiceCollection services, IConfiguration configuration)
    {
        services.ConfigureOptions<JwtOptionsSetup>();
        services.ConfigureOptions<JwtBearerOptionsSetup>();

        services
            .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer();
    }
}