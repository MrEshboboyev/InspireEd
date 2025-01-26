using InspireEd.Application.Abstractions.Messaging;
using InspireEd.Domain.Errors;
using InspireEd.Domain.Repositories;
using InspireEd.Domain.Shared;
using InspireEd.Domain.Users.Repositories;

namespace InspireEd.Application.Users.Commands.DeleteUser;

internal sealed class DeleteUserCommandHandler(
    IUserRepository userRepository,
    IUnitOfWork unitOfWork) : ICommandHandler<DeleteUserCommand>
{
    public async Task<Result> Handle(
        DeleteUserCommand request,
        CancellationToken cancellationToken)
    {
        var userId = request.UserId;
        
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
        
        #region Delete this User

        userRepository.Delete(user);
        await unitOfWork.SaveChangesAsync(cancellationToken);
        
        #endregion

        return Result.Success();
    }
}