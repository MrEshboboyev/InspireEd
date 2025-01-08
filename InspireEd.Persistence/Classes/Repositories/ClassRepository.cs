using InspireEd.Domain.Classes.Entities;
using InspireEd.Domain.Classes.Repositories;
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
}