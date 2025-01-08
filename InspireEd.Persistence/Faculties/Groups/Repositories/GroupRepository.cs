using InspireEd.Domain.Faculties.Entities;
using InspireEd.Domain.Faculties.Repositories;
using Microsoft.EntityFrameworkCore;

namespace InspireEd.Persistence.Faculties.Groups.Repositories;

public sealed class GroupRepository(ApplicationDbContext dbContext) : IGroupRepository
{
    private readonly ApplicationDbContext _dbContext = dbContext;

    public async Task<List<Group>> GetByIdsAsync(
        List<Guid> groupIds, 
        CancellationToken cancellationToken = default)
        => await _dbContext
            .Set<Group>()
            .AsNoTracking()
            .Where(g => groupIds.Contains(g.Id))
            .ToListAsync(cancellationToken);

    public void Add(Group group) => _dbContext.Set<Group>().Add(group);

    public void Update(Group group) => _dbContext.Set<Group>().Update(group);

    public void Remove(Group group) => _dbContext.Set<Group>().Remove(group);
}