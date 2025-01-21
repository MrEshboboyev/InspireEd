using InspireEd.Application.Abstractions.Messaging;
using InspireEd.Application.Users.Queries.Common;

namespace InspireEd.Application.Users.Queries.GetUsersByRole;

public sealed record GetUsersByRoleQuery(
    int RoleId) : IQuery<List<UserResponse>>;