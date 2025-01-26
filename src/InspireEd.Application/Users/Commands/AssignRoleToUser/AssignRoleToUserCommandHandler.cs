using InspireEd.Application.Abstractions.Messaging;
using InspireEd.Domain.Errors;
using InspireEd.Domain.Repositories;
using InspireEd.Domain.Shared;
using InspireEd.Domain.Users.Repositories;

namespace InspireEd.Application.Users.Commands.AssignRoleToUser;

internal sealed class AssignRoleToUserCommandHandler(
    IUserRepository userRepository,
    IRoleRepository roleRepository,
    IUnitOfWork unitOfWork) : ICommandHandler<AssignRoleToUserCommand>
{
    public async Task<Result> Handle(
        AssignRoleToUserCommand request,
        CancellationToken cancellationToken)
    {
        var (userId, roleId) = request;
        
        #region Get this User and Role
        
        var user = await userRepository.GetByIdAsync(
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
        
        #region Assign Role to User

        var assignRoleToUserResult = user.AssignRole(role);
        if (assignRoleToUserResult.IsSuccess)
        {
            return Result.Failure(
                assignRoleToUserResult.Error);
        }
        
        #endregion
        
        #region Update database
        
        userRepository.Update(user);
        await unitOfWork.SaveChangesAsync(cancellationToken);

        #endregion
        
        return Result.Success();
    }
}