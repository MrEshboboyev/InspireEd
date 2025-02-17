using InspireEd.Domain.Events;

namespace InspireEd.Domain.Users.Events;

public sealed record UserCreatedDomainEvent(
    Guid Id,
    Guid UserId,
    string Email) : DomainEvent(Id);