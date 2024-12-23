namespace InspireEd.Domain.Users.Entities;

public class DepartmentHead : UserRole
{
    public Guid FacultyId { get; private set; }

    private DepartmentHead(Guid userId, Guid facultyId)
    {
        UserId = userId;
        FacultyId = facultyId;
    }

    public static DepartmentHead Create(Guid userId, Guid facultyId)
    {
        return new DepartmentHead(userId, facultyId);
    }

    public void ChangeFaculty(Guid newFacultyId)
    {
        FacultyId = newFacultyId;
    }
}