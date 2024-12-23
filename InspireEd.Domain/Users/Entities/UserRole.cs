using InspireEd.Domain.Primitives;

namespace InspireEd.Domain.Users.Entities;

public abstract class UserRole : Entity
{
    public Guid UserId { get; protected set; }
}