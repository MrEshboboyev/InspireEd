using InspireEd.Application.Abstractions.Messaging;
using InspireEd.Domain.Users.Events;
using InspireEd.Domain.Users.Repositories;

namespace InspireEd.Application.Users.Events;

internal sealed class UserCreatedDomainEventHandler(
    IUserRepository userRepository)
        : IDomainEventHandler<UserCreatedDomainEvent>
{
    private readonly IUserRepository _userRepository = userRepository;

    public async Task Handle(
        UserCreatedDomainEvent notification,
        CancellationToken cancellationToken)
    {
        var user = await _userRepository.GetByIdAsync(notification.UserId, cancellationToken);

        if (user is null)
            return;

        Console.WriteLine($"User {notification.Email} has been created.");
    }
}