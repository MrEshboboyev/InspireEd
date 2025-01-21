using InspireEd.Application.Abstractions.Messaging;
using InspireEd.Application.Abstractions.Security;
using InspireEd.Domain.Errors;
using InspireEd.Domain.Repositories;
using InspireEd.Domain.Shared;
using InspireEd.Domain.Users.Repositories;

namespace InspireEd.Application.Users.Commands.ChangeUserPassword;

internal sealed class ChangeUserPasswordCommandHandler(
    IUserRepository userRepository,
    IPasswordHasher passwordHasher,
    IUnitOfWork unitOfWork) : ICommandHandler<ChangeUserPasswordCommand>
{
    public async Task<Result> Handle(
        ChangeUserPasswordCommand request,
        CancellationToken cancellationToken)
    {
        var (userId, oldPassword, newPassword) = request;
        
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

        #region Checking old password is coorect
        
        if (user.PasswordHash != passwordHasher.Hash(oldPassword))
        {
            return Result.Failure(
                DomainErrors.User.InvalidPassword);
        }
        
        #endregion
        
        #region Update this User's password
        
        user.ChangePassword(passwordHasher.Hash(newPassword));
        
        #endregion
        
        #region Update database
        
        userRepository.Update(user);
        await unitOfWork.SaveChangesAsync(cancellationToken);
        
        #endregion

        return Result.Success();
    }
}