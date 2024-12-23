namespace InspireEd.Domain.Users.Entities;

public class Student : UserRole
{
    public Guid GroupId { get; private set; }

    private Student(Guid userId, Guid groupId)
    {
        UserId = userId;
        GroupId = groupId;
    }

    public static Student Create(Guid userId, Guid groupId)
    {
        return new Student(userId, groupId);
    }

    public void ChangeGroup(Guid newGroupId)
    {
        GroupId = newGroupId;
    }
}