using InspireEd.Domain.Primitives;
using System.Reflection;

namespace InspireEd.ArchitectureTests;

public abstract class BaseTest
{
    protected static readonly Assembly DomainAssembly = typeof(Entity).Assembly;
}
