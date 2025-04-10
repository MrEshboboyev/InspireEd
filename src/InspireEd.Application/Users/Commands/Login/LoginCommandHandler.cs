using InspireEd.Application.Abstractions.Messaging;
using InspireEd.Application.Abstractions.Security;
using InspireEd.Domain.Errors;
using InspireEd.Domain.Shared;
using InspireEd.Domain.Users.Entities;
using InspireEd.Domain.Users.Repositories;
using InspireEd.Domain.Users.ValueObjects;

namespace InspireEd.Application.Users.Commands.Login;

/// <summary>
/// Handles the command to log in a user.
/// </summary>
internal sealed class LoginCommandHandler(
    IUserRepository userRepository,
    IJwtProvider jwtProvider,
    IPasswordHasher passwordHasher) : ICommandHandler<LoginCommand, string>
{
    /// <summary>
    /// Processes the LoginCommand and logs in the user by generating a JWT.
    /// </summary>
    /// <param name="request">The command request containing the user's email and password.</param>
    /// <param name="cancellationToken">Optional cancellation token.</param>
    /// <returns>A Result containing the JWT token if successful or an error.</returns>
    public async Task<Result<string>> Handle(LoginCommand request,
        CancellationToken cancellationToken)
    {
        var (email, password) = request;

        // Use LINQ-style query syntax for the login flow
        return await (
            from emailValue in Email.Create(email)
            from user in GetUserAsync(emailValue, cancellationToken)
            from verifiedUser in VerifyPassword(user, password)
            from token in GenerateTokenAsync(verifiedUser)
            select token
        );
    }

    // Helper methods to facilitate the LINQ approach

    private async Task<Result<User>> GetUserAsync(Email email, CancellationToken cancellationToken)
    {
        var user = await userRepository.GetByEmailAsync(email, cancellationToken);
        return user is not null
            ? Result.Success(user)
            : Result.Failure<User>(DomainErrors.User.InvalidCredentials);
    }

    private Task<Result<User>> VerifyPassword(User user, string password)
    {
        var result = passwordHasher.Verify(password, user.PasswordHash)
            ? Result.Success(user)
            : Result.Failure<User>(DomainErrors.User.InvalidCredentials);

        return Task.FromResult(result);
    }

    private async Task<Result<string>> GenerateTokenAsync(User user)
    {
        var token = await jwtProvider.GenerateAsync(user);
        return Result.Success(token);
    }
}
