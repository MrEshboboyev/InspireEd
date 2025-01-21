using InspireEd.Application.Abstractions.Messaging;
using InspireEd.Application.Users.Queries.Common;
using InspireEd.Domain.Users.Repositories;
using InspireEd.Domain.Shared;

namespace InspireEd.Application.Users.Queries.GetAllUsers;

internal sealed class GetAllUsersQueryHandler(
    IUserRepository userRepository) : IQueryHandler<GetAllUsersQuery, List<UserResponse>>
{
    public async Task<Result<List<UserResponse>>> Handle(
        GetAllUsersQuery request,
        CancellationToken cancellationToken)
    {
        var users = await userRepository.GetAllAsync(cancellationToken);
        
        var userResponses = users
            .Select(UserResponseFactory.Create)
            .ToList();

        return Result.Success(userResponses);
    }
}