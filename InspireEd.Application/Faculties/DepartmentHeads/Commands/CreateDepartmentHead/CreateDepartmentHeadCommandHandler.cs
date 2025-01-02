using InspireEd.Application.Abstractions.Messaging;
using InspireEd.Application.Abstractions.Security;
using InspireEd.Domain.Errors;
using InspireEd.Domain.Repositories;
using InspireEd.Domain.Shared;
using InspireEd.Domain.Users.Entities;
using InspireEd.Domain.Users.Repositories;
using InspireEd.Domain.Users.ValueObjects;

namespace InspireEd.Application.Faculties.DepartmentHeads.Commands.CreateDepartmentHead;

internal sealed class CreateDepartmentHeadCommandHandler(
    IUserRepository userRepository,
    IRoleRepository roleRepository,
    IPasswordHasher passwordHasher,
    IUnitOfWork unitOfWork) : ICommandHandler<CreateDepartmentHeadCommand>
{
    public async Task<Result> Handle(
        CreateDepartmentHeadCommand request,
        CancellationToken cancellationToken)
    {
        var (email, firstName, lastName, password) = request;
        
        #region Checking Email is Unique

        // Validate and create the Email value object
        Result<Email> emailResult = Email.Create(request.Email);

        // Check if the email is already in use
        if (!await userRepository.IsEmailUniqueAsync(emailResult.Value, cancellationToken))
        {
            return Result.Failure<Guid>(
                DomainErrors.User.EmailAlreadyInUse);
        }

        #endregion

        #region Prepare value objects

        // Validate and create the FirstName value object
        Result<FirstName> createFirstNameResult = FirstName.Create(request.FirstName);
        if (createFirstNameResult.IsFailure)
        {
            return Result.Failure<Guid>(
                createFirstNameResult.Error);
        }

        // Validate and create the LastName value object
        Result<LastName> createLastNameResult = LastName.Create(request.LastName);
        if (createLastNameResult.IsFailure)
        {
            return Result.Failure<Guid>(
                createFirstNameResult.Error);
        }

        #endregion

        #region Password hashing

        // Hash the user's password
        var passwordHash = passwordHasher.Hash(request.Password);

        #endregion
        
        #region Get DepartmentHead role from db
        
        var roleFromDb = await roleRepository.GetByNameAsync(Role.DepartmentHead.ToString(),
            cancellationToken);
        if (roleFromDb is null)
        {
            return Result.Failure<Guid>(
                DomainErrors.User.InvalidRoleName);
        }
        
        #endregion

        #region Create new DepartmentHead

        var user = User.Create(
            Guid.NewGuid(),
            emailResult.Value,
            passwordHash,
            createFirstNameResult.Value,
            createLastNameResult.Value, 
            roleFromDb); 

        #endregion

        #region Add and Update database

        // Add the new user to the repository and save changes
        userRepository.Add(user);
        await unitOfWork.SaveChangesAsync(cancellationToken);

        #endregion

        return Result.Success();
    }
}