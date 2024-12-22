using InspireEd.Application.Abstractions.Messaging;

namespace InspireEd.Application.Users.Queries.GetUserById;

/// <summary>
/// Query to retrieve a user by their unique identifier.
/// </summary>
/// <param name="UserId">The unique identifier of the user to be retrieved.</param>
public sealed record GetUserByIdQuery(Guid UserId) : IQuery<UserResponse>;