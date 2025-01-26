using InspireEd.Application.Abstractions.Messaging;
using InspireEd.Domain.Errors;
using InspireEd.Domain.Repositories;
using InspireEd.Domain.Shared;
using InspireEd.Domain.Users.Repositories;
using InspireEd.Domain.Users.ValueObjects;

namespace InspireEd.Application.Users.Commands.UpdateUser;

internal sealed class UpdateUserCommandHandler(
    IUserRepository userRepository,
    IUnitOfWork unitOfWork) : ICommandHandler<UpdateUserCommand>
{
    public async Task<Result> Handle(
        UpdateUserCommand request,
        CancellationToken cancellationToken)
    {
        var (userId, firstName, lastName, email) = request;
        
        #region Get this User
        
        var user = await userRepository.GetByIdAsync(
            userId,
            cancellationToken);
        if (user is null)
        {
            return Result.Failure(
                DomainErrors.User.NotFound(request.UserId));
        }
        
        #endregion

        #region Prepare value objects
        
        var firstNameResult = FirstName.Create(firstName);
        if (firstNameResult.IsFailure)
        {
            return Result.Failure(firstNameResult.Error);
        }

        var lastNameResult = LastName.Create(lastName);
        if (lastNameResult.IsFailure)
        {
            return Result.Failure(lastNameResult.Error);
        }

        var emailResult = Email.Create(email);
        if (emailResult.IsFailure)
        {
            return Result.Failure(emailResult.Error);
        }
        
        #endregion
        
        #region Update this user

        var updateResult = user.UpdateDetails(
            firstNameResult.Value,
            lastNameResult.Value,
            emailResult.Value);
        if (updateResult.IsFailure)
        {
            return Result.Failure(
                updateResult.Error);
        }
        
        #endregion
        
        #region Update database

        userRepository.Update(user);
        await unitOfWork.SaveChangesAsync(cancellationToken);
        
        #endregion

        return Result.Success();
    }
}