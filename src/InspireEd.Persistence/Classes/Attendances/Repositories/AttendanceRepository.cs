using InspireEd.Domain.Classes.Entities;
using InspireEd.Domain.Classes.Repositories;
using Microsoft.EntityFrameworkCore;

namespace InspireEd.Persistence.Classes.Attendances.Repositories;

public class AttendanceRepository(ApplicationDbContext dbContext) : IAttendanceRepository
{
    public async Task<Attendance> GetByIdAsync(
        Guid id,
        CancellationToken cancellationToken = default)
        => await dbContext
            .Set<Attendance>()
            .AsNoTracking()
            .SingleOrDefaultAsync(x => x.Id == id, cancellationToken);

    public void Add(Attendance attendance) => dbContext.Set<Attendance>().Add(attendance);

    public void Update(Attendance attendance) => dbContext.Set<Attendance>().Update(attendance);
}