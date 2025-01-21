using InspireEd.Domain.Users.Entities;

namespace InspireEd.Domain.Users.Repositories;

public interface IRoleRepository
{
    Task<Role> GetByIdAsync(int id, CancellationToken cancellationToken = default);
    Task<Role> GetByNameAsync(string name, CancellationToken cancellationToken = default);
}