using InspireEd.Domain.Classes.Entities;
using InspireEd.Domain.Classes.Repositories;
using InspireEd.Domain.Faculties.Entities;
using Microsoft.EntityFrameworkCore;

namespace InspireEd.Persistence.Classes.Repositories;

public sealed class ClassRepository(ApplicationDbContext dbContext) : IClassRepository
{
    public async Task<List<Class>> GetAllAsync(
        CancellationToken cancellationToken = default)
        => await dbContext
            .Set<Class>()
            .AsNoTracking()
            .ToListAsync(cancellationToken);
    
    public async Task<Class> GetByIdAsync(
        Guid id,
        CancellationToken cancellationToken = default)
        => await dbContext
            .Set<Class>()
            .AsNoTracking()
            .SingleOrDefaultAsync(x => x.Id == id, cancellationToken);
    
    public async Task<Class> GetByIdWithAttendancesAsync(
        Guid id,
        CancellationToken cancellationToken = default)
        => await dbContext
            .Set<Class>()
            .Include(c => c.Attendances)
            .AsNoTracking()
            .SingleOrDefaultAsync(x => x.Id == id, cancellationToken);

    public void Add(Class classItem) => dbContext.Set<Class>().Add(classItem);

    public void Update(Class classItem) => dbContext.Set<Class>().Update(classItem);

    public void Remove(Class classItem) => dbContext.Set<Class>().Remove(classItem);

    public async Task<List<Guid>> GetGroupStudentIds(
        Guid classId,
        CancellationToken cancellationToken = default)
    {
        var classEntity = await dbContext
            .Set<Class>()
            .AsNoTracking()
            .SingleOrDefaultAsync(x => x.Id == classId, cancellationToken);

        if (classEntity is null)
        {
            return [];
        }

        return classEntity.GroupIds
            .SelectMany(groupId => dbContext.Set<Group>()
                .Where(g => g.Id == groupId)
                .SelectMany(g => g.StudentIds))
            .ToList();
    }

    public async Task<List<Class>> GetBySubjectIdAsync(
        Guid subjectId,
        CancellationToken cancellationToken = default)
        => await dbContext
            .Set<Class>()
            .AsNoTracking()
            .Where(x => x.SubjectId == subjectId)
            .ToListAsync(cancellationToken);
}