using InspireEd.Application.Abstractions.Messaging;
using InspireEd.Application.Abstractions.Security;
using InspireEd.Domain.Errors;
using InspireEd.Domain.Repositories;
using InspireEd.Domain.Shared;
using InspireEd.Domain.Users.Repositories;

namespace InspireEd.Application.Users.Commands.ChangeUserPassword;

internal sealed class UpdateUserPasswordCommandHandler(
    IUserRepository userRepository,
    IPasswordHasher passwordHasher,
    IUnitOfWork unitOfWork) : ICommandHandler<UpdateUserPasswordCommand>
{
    public async Task<Result> Handle(
        UpdateUserPasswordCommand request,
        CancellationToken cancellationToken)
    {
        var (userId, newPassword) = request;
        
        #region Get this User
        
        var user = await userRepository.GetByIdAsync(
            userId,
            cancellationToken);
        if (user is null)
        {
            return Result.Failure(
                DomainErrors.User.NotFound(userId));
        }
        
        #endregion
        
        #region Update this User's password
        
        var changePasswordResult = user.ChangePassword(passwordHasher.Hash(newPassword));
        if (changePasswordResult.IsFailure)
        {
            return Result.Failure(
                changePasswordResult.Error);
        }
        
        #endregion
        
        #region Update database
        
        userRepository.Update(user);
        await unitOfWork.SaveChangesAsync(cancellationToken);
        
        #endregion

        return Result.Success();
    }
}