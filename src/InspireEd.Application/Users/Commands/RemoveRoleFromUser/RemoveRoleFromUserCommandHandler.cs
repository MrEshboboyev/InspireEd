using InspireEd.Application.Abstractions.Messaging;
using InspireEd.Domain.Errors;
using InspireEd.Domain.Repositories;
using InspireEd.Domain.Shared;
using InspireEd.Domain.Users.Repositories;

namespace InspireEd.Application.Users.Commands.RemoveRoleFromUser;

internal sealed class RemoveRoleFromUserCommandHandler(
    IUserRepository userRepository,
    IRoleRepository roleRepository,
    IUnitOfWork unitOfWork) : ICommandHandler<RemoveRoleFromUserCommand>
{
    public async Task<Result> Handle(
        RemoveRoleFromUserCommand request,
        CancellationToken cancellationToken)
    {
        var (userId, roleId) = request;
        
        #region Get this User and Role
        
        var user = await userRepository.GetByIdWithRolesAsync(
            userId,
            cancellationToken);
        if (user is null)
        {
            return Result.Failure(
                DomainErrors.User.NotFound(userId));
        }

        var role = await roleRepository.GetByIdAsync(
            roleId,
            cancellationToken);
        if (role is null)
        {
            return Result.Failure(
                DomainErrors.Role.NotFound(roleId));
        }
        
        #endregion
        
        #region Remove role from user

        var removeRoleFromUserResult = user.RemoveRole(role);
        if (removeRoleFromUserResult.IsFailure)
        {
            return Result.Failure(
                removeRoleFromUserResult.Error);
        }
        
        #endregion
        
        #region Update database
        
        userRepository.Update(user);
        await unitOfWork.SaveChangesAsync(cancellationToken);

        #endregion
        
        return Result.Success();
    }
}