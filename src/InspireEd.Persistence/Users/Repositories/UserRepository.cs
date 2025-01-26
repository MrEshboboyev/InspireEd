using InspireEd.Domain.Users.Entities;
using InspireEd.Domain.Users.Repositories;
using InspireEd.Domain.Users.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.SqlServer.Server;

namespace InspireEd.Persistence.Users.Repositories;

public sealed class UserRepository(ApplicationDbContext dbContext) : IUserRepository
{
    public async Task<List<User>> SearchByNameAsync(string searchTerm, CancellationToken cancellationToken)
    {
        return await dbContext
            .Set<User>()
            .Where(user => EF.Functions.Like(user.FirstName.Value, $"%{searchTerm}%")
                           || EF.Functions.Like(user.LastName.Value, $"%{searchTerm}%"))
            .ToListAsync(cancellationToken);
    }
    
    public async Task<List<User>> GetAllAsync(CancellationToken cancellationToken = default)
        => await dbContext.Set<User>().ToListAsync(cancellationToken);

    public async Task<User> GetByIdAsync(Guid id, CancellationToken cancellationToken = default) =>
        await dbContext
            .Set<User>()
            .FirstOrDefaultAsync(user => user.Id == id, cancellationToken);

    public async Task<List<User>> GetByIdsAsync(List<Guid> ids, CancellationToken cancellationToken = default) =>
            await dbContext
                .Set<User>()
                .Where(user => ids.Contains(user.Id))
                .ToListAsync(cancellationToken);

    public async Task<User> GetByEmailAsync(Email email, CancellationToken cancellationToken = default) =>
        await dbContext
            .Set<User>()
            .FirstOrDefaultAsync(user => user.Email == email, cancellationToken);

    public async Task<bool> IsEmailUniqueAsync(Email email, CancellationToken cancellationToken = default) =>
        !await dbContext
            .Set<User>()
            .AnyAsync(user => user.Email == email, cancellationToken);

    public void Add(User user) =>
        dbContext.Set<User>().Add(user);

    public void Update(User user) =>
        dbContext.Set<User>().Update(user);

    public void Delete(User user) =>
        dbContext.Set<User>().Remove(user);

    public async Task<List<User>> GetByRoleIdAsync(
        int roleId, 
        CancellationToken cancellationToken)
    {
        return await dbContext
            .Set<User>()
            .Where(user => user.Roles.Count(role => role.Id == roleId) == 1)
            .ToListAsync(cancellationToken);
    }
}
