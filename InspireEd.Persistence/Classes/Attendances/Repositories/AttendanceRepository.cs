using InspireEd.Domain.Classes.Entities;
using InspireEd.Domain.Classes.Repositories;

namespace InspireEd.Persistence.Classes.Attendances.Repositories;

public class AttendanceRepository(ApplicationDbContext dbContext) : IAttendanceRepository
{
    private readonly ApplicationDbContext _dbContext = dbContext;

    public void Add(Attendance attendance) => _dbContext.Set<Attendance>().Add(attendance);

    public void Update(Attendance attendance) => _dbContext.Set<Attendance>().Update(attendance);
}