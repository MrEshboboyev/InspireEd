using InspireEd.Domain.Faculties.Entities;
using InspireEd.Domain.Faculties.Repositories;

namespace InspireEd.Persistence.Faculties.Groups.Repositories;

public sealed class GroupRepository(ApplicationDbContext dbContext) : IGroupRepository
{
    private readonly ApplicationDbContext _dbContext = dbContext;

    public void Add(Group group) => _dbContext.Set<Group>().Add(group);

    public void Update(Group group) => _dbContext.Set<Group>().Update(group);

    public void Remove(Group group) => _dbContext.Set<Group>().Remove(group);
}