using InspireEd.Application.Abstractions.Messaging;
using InspireEd.Application.Users.Queries.Common;
using InspireEd.Domain.Errors;
using InspireEd.Domain.Shared;
using InspireEd.Domain.Users.Repositories;

namespace InspireEd.Application.Users.Queries.SearchUsersByName;

/// <summary>
/// Query handler for searching users by name.
/// </summary>
internal sealed class SearchUsersByNameQueryHandler(
    IUserRepository userRepository) : IQueryHandler<SearchUsersByNameQuery, List<UserResponse>>
{
    public async Task<Result<List<UserResponse>>> Handle(
        SearchUsersByNameQuery request,
        CancellationToken cancellationToken)
    {
        var searchTerm = request.SearchTerm;

        #region Get Users by Search Term

        // Fetch users based on the search term from the repository
        var users = await userRepository.SearchByNameAsync(searchTerm, cancellationToken);

        if (users is null || users.Count == 0)
        {
            return Result.Failure<List<UserResponse>>(
                DomainErrors.User.NoUsersFoundForSearchTerm(searchTerm));
        }

        // Map domain users to response DTOs using the factory
        var userResponses = users
            .Select(UserResponseFactory.Create)
            .ToList();

        #endregion

        return Result.Success(userResponses);
    }
}