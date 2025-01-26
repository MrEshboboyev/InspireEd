using InspireEd.Application.Abstractions.Security;
using InspireEd.Application.Abstractions.Messaging;
using InspireEd.Domain.Errors;
using InspireEd.Domain.Repositories;
using InspireEd.Domain.Shared;
using InspireEd.Domain.Users.Entities;
using InspireEd.Domain.Users.Repositories;
using InspireEd.Domain.Users.ValueObjects;

namespace InspireEd.Application.Users.Commands.CreateUser;

/// <summary>
/// Handles the command to create a new user.
/// </summary>
internal sealed class CreateUserCommandHandler(
    IUserRepository userRepository,
    IRoleRepository roleRepository,
    IUnitOfWork unitOfWork,
    IPasswordHasher passwordHasher)
    : ICommandHandler<CreateUserCommand, Guid>
{
    /// <summary>
    /// Processes the CreateUserCommand and creates a new user.
    /// </summary>
    /// <param name="request">The command request containing user details.</param>
    /// <param name="cancellationToken">Optional cancellation token.</param>
    /// <returns>A Result containing the unique identifier of the created user or an error.</returns>
    public async Task<Result<Guid>> Handle(CreateUserCommand request,
        CancellationToken cancellationToken)
    {
        var (email, password, firstName, lastName, roleName) = request;
        
        #region Checking Email is Unique

        // Validate and create the Email value object
        var emailResult = Email.Create(email);
        if (emailResult.IsFailure)
        {
            return Result.Failure<Guid>(
                emailResult.Error);
        }

        // Check if the email is already in use
        if (!await userRepository.IsEmailUniqueAsync(emailResult.Value, cancellationToken))
        {
            return Result.Failure<Guid>(
                DomainErrors.User.EmailAlreadyInUse);
        }

        #endregion

        #region Prepare value objects

        // Validate and create the FirstName value object
        var createFirstNameResult = FirstName.Create(firstName);
        if (createFirstNameResult.IsFailure)
        {
            return Result.Failure<Guid>(
                createFirstNameResult.Error);
        }

        // Validate and create the LastName value object
        var createLastNameResult = LastName.Create(lastName);
        if (createLastNameResult.IsFailure)
        {
            return Result.Failure<Guid>(
                createLastNameResult.Error);
        }

        #endregion

        #region Password hashing

        // Hash the user's password
        var passwordHash = passwordHasher.Hash(request.Password);

        #endregion
        
        #region Convert Role Name to Role

        // Convert the RoleName string into a Role instance using FromName
        var role = Role.FromName(request.RoleName);
        if (role == null)
        {
            return Result.Failure<Guid>(
                DomainErrors.User.InvalidRoleName);
        }

        #endregion
        
        #region Get this role from db
        
        var roleFromDb = await roleRepository.GetByNameAsync(request.RoleName,
            cancellationToken);
        if (roleFromDb is null)
        {
            return Result.Failure<Guid>(
                DomainErrors.User.InvalidRoleName);
        }
        
        #endregion

        #region Create new user

        // Create a new User entity with the provided details
        var user = User.Create(
            Guid.NewGuid(),
            emailResult.Value,
            passwordHash,
            createFirstNameResult.Value,
            createLastNameResult.Value, 
            roleFromDb); // fix this coming soon

        #endregion

        #region Add and Update database

        // Add the new user to the repository and save changes
        userRepository.Add(user);
        await unitOfWork.SaveChangesAsync(cancellationToken);

        #endregion

        // Return the unique identifier of the newly created user
        return Result.Success(user.Id);
    }
}