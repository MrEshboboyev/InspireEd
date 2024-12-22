using InspireEd.Application.Abstractions;
using InspireEd.Application.Abstractions.Messaging;
using InspireEd.Domain.Entities;
using InspireEd.Domain.Errors;
using InspireEd.Domain.Repositories;
using InspireEd.Domain.Shared;
using InspireEd.Domain.ValueObjects;

namespace InspireEd.Application.Users.Commands.CreateUser;

internal sealed class CreateUserCommandHandler : ICommandHandler<CreateUserCommand, Guid>
{
    private readonly IUserRepository _userRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IPasswordHasher _passwordHasher;

    public CreateUserCommandHandler(
        IUserRepository userRepository,
        IUnitOfWork unitOfWork,
        IPasswordHasher passwordHasher)
    {
        _userRepository = userRepository;
        _unitOfWork = unitOfWork;
        _passwordHasher = passwordHasher;
    }

    public async Task<Result<Guid>> Handle(CreateUserCommand request, CancellationToken cancellationToken)
    {
        Result<Email> emailResult = Email.Create(request.Email);
        Result<FirstName> firstNameResult = FirstName.Create(request.FirstName);
        Result<LastName> lastNameResult = LastName.Create(request.LastName);
        string passwordHash = _passwordHasher.Hash(request.Password);

        if (!await _userRepository.IsEmailUniqueAsync(emailResult.Value, cancellationToken))
        {
            return Result.Failure<Guid>(DomainErrors.User.EmailAlreadyInUse);
        }

        var user = User.Create(
            Guid.NewGuid(),
            emailResult.Value,
            passwordHash,
            firstNameResult.Value,
            lastNameResult.Value);

        _userRepository.Add(user);

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return user.Id;
    }
}