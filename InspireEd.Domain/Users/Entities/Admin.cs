namespace InspireEd.Domain.Users.Entities;

public class Admin : UserRole
{
    private Admin(Guid userId)
    {
        UserId = userId;
    }

    public static Admin Create(Guid userId)
    {
        return new Admin(userId);
    }
}