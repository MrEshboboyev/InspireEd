using InspireEd.Application.Abstractions.Messaging;

namespace InspireEd.Application.Users.Queries.GetUserById;

public sealed record GetUserByIdQuery(Guid UserId) : IQuery<UserResponse>;