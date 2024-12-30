using InspireEd.Application.Abstractions.Security;
using InspireEd.Domain.Errors;
using InspireEd.Domain.Repositories;
using InspireEd.Domain.Shared;
using InspireEd.Domain.Users.Entities;
using InspireEd.Domain.Users.Repositories;
using InspireEd.Domain.Users.ValueObjects;

namespace InspireEd.Application.Users.Services;

public class UserCreationService(
    IUserRepository userRepository,
    IPasswordHasher passwordHasher,
    IRoleRepository roleRepository,
    IUnitOfWork unitOfWork)
    : IUserCreationService
{
    public async Task<Result<Guid>> CreateUserAsync(
        string firstName,
        string lastName,
        string email,
        string password,
        string? roleName,
        CancellationToken cancellationToken)
    {
        #region Checking Email is Unique

        var emailResult = Email.Create(email);
        if (!emailResult.IsSuccess)
        {
            return Result.Failure<Guid>(emailResult.Error);
        }

        if (!await userRepository.IsEmailUniqueAsync(emailResult.Value, cancellationToken))
        {
            return Result.Failure<Guid>(DomainErrors.User.EmailAlreadyInUse);
        }

        #endregion

        #region Prepare value objects

        var createFirstNameResult = FirstName.Create(firstName);
        if (!createFirstNameResult.IsSuccess)
        {
            return Result.Failure<Guid>(createFirstNameResult.Error);
        }

        var createLastNameResult = LastName.Create(lastName);
        if (!createLastNameResult.IsSuccess)
        {
            return Result.Failure<Guid>(createLastNameResult.Error);
        }

        #endregion

        #region Password Hashing

        var passwordHash = passwordHasher.Hash(password);

        #endregion

        #region Convert Role Name to Role

        var roleFromDb = await roleRepository.GetByNameAsync(roleName, cancellationToken);
        if (roleFromDb is null)
        {
            return Result.Failure<Guid>(
                DomainErrors.User.InvalidRoleName);
        }

        #endregion

        #region Create New User

        var user = User.Create(
            Guid.NewGuid(),
            emailResult.Value,
            passwordHash,
            createFirstNameResult.Value,
            createLastNameResult.Value,
            roleFromDb);

        userRepository.Add(user);

        #endregion

        #region Save Changes

        await unitOfWork.SaveChangesAsync(cancellationToken);

        #endregion

        return Result.Success(user.Id);
    }
}
