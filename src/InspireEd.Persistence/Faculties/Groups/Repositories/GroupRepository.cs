using InspireEd.Domain.Faculties.Entities;
using InspireEd.Domain.Faculties.Repositories;
using Microsoft.EntityFrameworkCore;

namespace InspireEd.Persistence.Faculties.Groups.Repositories;

public sealed class GroupRepository(ApplicationDbContext dbContext) : IGroupRepository
{
    public async Task<List<Group>> GetByIdsAsync(
        List<Guid> groupIds, 
        CancellationToken cancellationToken = default)
        => await dbContext
            .Set<Group>()
            .AsNoTracking()
            .Where(g => groupIds.Contains(g.Id))
            .ToListAsync(cancellationToken);
    
    public async Task<Group> GetByIdAsync(
        Guid id, 
        CancellationToken cancellationToken = default)
        => await dbContext
            .Set<Group>()
            .AsNoTracking()
            .SingleOrDefaultAsync(g => g.Id == id, cancellationToken);

    public void Add(Group group) => dbContext.Set<Group>().Add(group);

    public void Update(Group group) => dbContext.Set<Group>().Update(group);

    public void Remove(Group group) => dbContext.Set<Group>().Remove(group);
}