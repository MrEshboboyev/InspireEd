using InspireEd.Domain.Classes.Entities;
using InspireEd.Domain.Classes.Repositories;
using InspireEd.Domain.Faculties.Entities;
using Microsoft.EntityFrameworkCore;

namespace InspireEd.Persistence.Classes.Repositories;

public sealed class ClassRepository(ApplicationDbContext dbContext) : IClassRepository
{
    private readonly ApplicationDbContext _dbContext = dbContext;

    public async Task<Class> GetByIdAsync(
        Guid id,
        CancellationToken cancellationToken = default)
        => await _dbContext
            .Set<Class>()
            .AsNoTracking()
            .SingleOrDefaultAsync(x => x.Id == id, cancellationToken);

    public void Add(Class classItem) => _dbContext.Set<Class>().Add(classItem);

    public void Update(Class classItem) => _dbContext.Set<Class>().Update(classItem);

    public void Remove(Class classItem) => _dbContext.Set<Class>().Remove(classItem);

    public async Task<List<Guid>> GetGroupStudentIds(
        Guid classId,
        CancellationToken cancellationToken = default)
    {
        var classEntity = await _dbContext
            .Set<Class>()
            .AsNoTracking()
            .SingleOrDefaultAsync(x => x.Id == classId, cancellationToken);

        if (classEntity is null)
        {
            return [];
        }

        return classEntity.GroupIds
            .SelectMany(groupId => _dbContext.Set<Group>()
                .Where(g => g.Id == groupId)
                .SelectMany(g => g.StudentIds))
            .ToList();
    }
}