namespace InspireEd.Domain.Users.Entities;

public class Teacher : UserRole
{
    private Teacher(Guid userId)
    {
        UserId = userId;
    }

    public static Teacher Create(Guid userId)
    {
        return new Teacher(userId);
    }
}