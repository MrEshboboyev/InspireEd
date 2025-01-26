using InspireEd.Domain.Users.Entities;
using InspireEd.Domain.Users.Repositories;
using Microsoft.EntityFrameworkCore;

namespace InspireEd.Persistence.Users.Repositories;

public class RoleRepository(ApplicationDbContext dbContext) : IRoleRepository
{
    public async Task<Role> GetByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        return await dbContext
            .Set<Role>()
            .FirstOrDefaultAsync(r => r.Id.Equals(id), cancellationToken);
    }
    
    public async Task<Role> GetByNameAsync(string name, CancellationToken cancellationToken = default)
    {
        return await dbContext.Set<Role>().FirstOrDefaultAsync(r => 
            r.Name.Equals(name), cancellationToken);
    }
}