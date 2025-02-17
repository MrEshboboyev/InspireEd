using Assembly = System.Reflection.Assembly;

namespace InspireEd.ArchitectureTests;

public abstract class BaseTest
{
    protected static readonly Assembly DomainAssembly = typeof(InspireEd.Domain.AssemblyReference).Assembly;
    protected static readonly Assembly ApplicationAssembly = typeof(InspireEd.Application.AssemblyReference).Assembly;
    protected static readonly Assembly PersistenceAssembly = typeof(Persistence.AssemblyReference).Assembly;
    protected static readonly Assembly InfrastructureAssembly = typeof(Infrastructure.AssemblyReference).Assembly;
    protected static readonly Assembly PresentationAssembly = typeof(Presentation.AssemblyReference).Assembly;
}
