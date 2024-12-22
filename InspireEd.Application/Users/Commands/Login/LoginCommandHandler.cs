using InspireEd.Application.Abstractions;
using InspireEd.Application.Abstractions.Messaging;
using InspireEd.Domain.Entities;
using InspireEd.Domain.Errors;
using InspireEd.Domain.Repositories;
using InspireEd.Domain.Shared;
using InspireEd.Domain.ValueObjects;

namespace InspireEd.Application.Users.Commands.Login;

internal sealed class LoginCommandHandler(IUserRepository userRepository,
    IJwtProvider jwtProvider, IPasswordHasher passwordHasher) : ICommandHandler<LoginCommand, string>
{
    private readonly IUserRepository _userRepository = userRepository;
    private readonly IJwtProvider _jwtProvider = jwtProvider;
    private readonly IPasswordHasher _passwordHasher = passwordHasher;

    public async Task<Result<string>> Handle(LoginCommand request, CancellationToken cancellationToken)
    {
        Result<Email> email = Email.Create(request.Email);

        User user = await _userRepository.GetByEmailAsync(
            email.Value,
            cancellationToken);

        if (user is null || !_passwordHasher.Verify(request.Password, user.PasswordHash))
        {
            return Result.Failure<string>(
                DomainErrors.User.InvalidCredentials);
        }

        string token = await _jwtProvider.GenerateAsync(user);
        return token;
    }
}

