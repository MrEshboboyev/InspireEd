using InspireEd.Domain.Users.Entities;

namespace InspireEd.Application.Users.Queries.Common;

public static class UserResponseFactory
{
    public static UserResponse Create(User user)
    {
        return new UserResponse(
            user.Id,
            user.FirstName.Value,
            user.LastName.Value,
            user.Email.Value,
            user.CreatedOnUtc,
            user.ModifiedOnUtc);
    }
}