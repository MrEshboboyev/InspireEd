using InspireEd.Application.Abstractions.Messaging;
using InspireEd.Application.Users.Queries.Common;

namespace InspireEd.Application.Users.Queries.SearchUsersByName;

public sealed record SearchUsersByNameQuery(
    string SearchTerm) : IQuery<List<UserResponse>>;