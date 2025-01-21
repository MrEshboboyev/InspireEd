using InspireEd.Application.Abstractions.Messaging;
using InspireEd.Application.Users.Queries.Common;
using InspireEd.Domain.Errors;
using InspireEd.Domain.Shared;
using InspireEd.Domain.Users.Repositories;

namespace InspireEd.Application.Users.Queries.GetUsersByRole;

internal sealed class GetUsersByRoleQueryHandler(
    IUserRepository userRepository) : IQueryHandler<GetUsersByRoleQuery, List<UserResponse>>
{
    public async Task<Result<List<UserResponse>>> Handle(
        GetUsersByRoleQuery request,
        CancellationToken cancellationToken)
    {
        var roleId = request.RoleId;
        
        #region Get this Users
        
        var users = await userRepository.GetByRoleIdAsync(
            roleId,
            cancellationToken);
        if (users.Count == 0)
        {
            return Result.Failure<List<UserResponse>>(
                DomainErrors.User.NoUsersFoundForRole(request.RoleId));
        }
        
        #endregion
        
        #region Prepare response
        
        var userResponses = users
            .Select(UserResponseFactory.Create)
            .ToList();
        
        #endregion

        return Result.Success(userResponses);
    }
}