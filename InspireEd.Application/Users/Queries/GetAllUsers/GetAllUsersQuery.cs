using InspireEd.Application.Abstractions.Messaging;
using InspireEd.Application.Users.Queries.Common;

namespace InspireEd.Application.Users.Queries.GetAllUsers;

public sealed record GetAllUsersQuery : IQuery<List<UserResponse>>;