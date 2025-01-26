using InspireEd.Domain.Shared;

namespace InspireEd.Application.Users.Services;

public interface IUserCreationService
{
    Task<Result<Guid>> CreateUserAsync(
        string firstName,
        string lastName,
        string email,
        string password,
        string? roleName,
        CancellationToken cancellationToken);
}