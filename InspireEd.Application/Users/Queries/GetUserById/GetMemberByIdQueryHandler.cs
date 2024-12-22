using InspireEd.Application.Abstractions.Messaging;
using InspireEd.Domain.Errors;
using InspireEd.Domain.Repositories;
using InspireEd.Domain.Shared;

namespace InspireEd.Application.Users.Queries.GetUserById;

internal sealed class GetUserByIdQueryHandler(IUserRepository userRepository)
        : IQueryHandler<GetUserByIdQuery, UserResponse>
{
    private readonly IUserRepository _userRepository = userRepository;

    public async Task<Result<UserResponse>> Handle
        (GetUserByIdQuery request,
        CancellationToken cancellationToken)
    {
        var user = await _userRepository.GetByIdAsync(
            request.UserId,
            cancellationToken);
        if (user is null)
        {
            return Result.Failure<UserResponse>(
                          DomainErrors.User.NotFound(request.UserId));
        }
        var response = new UserResponse(user.Id, user.Email.Value, user.FirstName.Value,
            user.LastName.Value);
        return response;
    }
}


