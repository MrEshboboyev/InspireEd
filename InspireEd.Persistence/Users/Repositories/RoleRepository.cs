using InspireEd.Domain.Users.Entities;
using InspireEd.Domain.Users.Repositories;
using Microsoft.EntityFrameworkCore;

namespace InspireEd.Persistence.Users.Repositories;

public class RoleRepository(ApplicationDbContext dbContext) : IRoleRepository
{
    private readonly ApplicationDbContext _dbContext = dbContext;

    public async Task<Role> GetByNameAsync(string name, CancellationToken cancellationToken = default)
    {
        return await _dbContext.Set<Role>().FirstOrDefaultAsync(r => 
            r.Name.Equals(name), cancellationToken);
    }
}