using InspireEd.Application.Abstractions.Messaging;
using InspireEd.Application.Users.Queries.Common;
using InspireEd.Domain.Errors;
using InspireEd.Domain.Shared;
using InspireEd.Domain.Users.Repositories;

namespace InspireEd.Application.Users.Queries.GetUserRoles;

internal sealed class GetUserRolesQueryHandler(
    IUserRepository userRepository) : IQueryHandler<GetUserRolesQuery, List<RoleResponse>>
{
    public async Task<Result<List<RoleResponse>>> Handle(
        GetUserRolesQuery request,
        CancellationToken cancellationToken)
    {
        var user = await userRepository.GetByIdAsync(request.UserId, cancellationToken);

        if (user is null)
        {
            return Result.Failure<List<RoleResponse>>(
                DomainErrors.User.NotFound(request.UserId));
        }

        var roleResponses = user.Roles
            .Select(RoleResponseFactory.Create)
            .ToList();

        return Result.Success(roleResponses);
    }
}