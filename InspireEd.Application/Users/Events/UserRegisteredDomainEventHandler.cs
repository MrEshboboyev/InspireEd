using InspireEd.Application.Abstractions.Messaging;
using InspireEd.Domain.Entities;
using InspireEd.Domain.Events;
using InspireEd.Domain.Repositories;

namespace InspireEd.Application.Users.Events;

internal sealed class UserRegisteredDomainEventHandler(
    IUserRepository userRepository)
          : IDomainEventHandler<UserRegisteredDomainEvent>
{
    private readonly IUserRepository _userRepository = userRepository;

    public async Task Handle(
        UserRegisteredDomainEvent notification,
        CancellationToken cancellationToken)
    {
        User user = await _userRepository.GetByIdAsync(
            notification.UserId,
            cancellationToken);

        if (user is null)
        {
            return;
        }
    }
}
