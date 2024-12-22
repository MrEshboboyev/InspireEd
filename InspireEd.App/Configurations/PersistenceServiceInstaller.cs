using InspireEd.Persistence.Interceptors;
using InspireEd.Persistence;
using Microsoft.EntityFrameworkCore;
using InspireEd.Domain.Repositories;
using InspireEd.Persistence.Repositories;

namespace InspireEd.App.Configurations;

public class PersistenceServiceInstaller : IServiceInstaller
{
    public void Install(IServiceCollection services, IConfiguration configuration)
    {
        services.AddSingleton<ConvertDomainEventsToOutboxMessagesInterceptor>();
        services.AddDbContext<ApplicationDbContext>(
            options => options.UseSqlServer(
                    configuration.GetConnectionString("Database")));
    }
}