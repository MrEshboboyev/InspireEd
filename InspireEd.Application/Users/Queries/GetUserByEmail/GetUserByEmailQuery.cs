using InspireEd.Application.Abstractions.Messaging;
using InspireEd.Application.Users.Queries.Common;

namespace InspireEd.Application.Users.Queries.GetUserByEmail;

public sealed record GetUserByEmailQuery(
    string Email) : IQuery<UserResponse>;