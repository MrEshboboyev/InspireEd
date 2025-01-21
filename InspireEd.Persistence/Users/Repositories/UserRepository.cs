﻿using InspireEd.Domain.Users.Entities;
using InspireEd.Domain.Users.Repositories;
using InspireEd.Domain.Users.ValueObjects;
using Microsoft.EntityFrameworkCore;

namespace InspireEd.Persistence.Users.Repositories;

public sealed class UserRepository(ApplicationDbContext dbContext) : IUserRepository
{
    private readonly ApplicationDbContext _dbContext = dbContext;

    public async Task<List<User>> GetUsersAsync(CancellationToken cancellationToken = default)
        => await _dbContext.Set<User>().ToListAsync(cancellationToken);

    public async Task<User> GetByIdAsync(Guid id, CancellationToken cancellationToken = default) =>
        await _dbContext
            .Set<User>()
            .FirstOrDefaultAsync(user => user.Id == id, cancellationToken);

    public async Task<List<User>> GetByIdsAsync(List<Guid> ids, CancellationToken cancellationToken = default) =>
            await _dbContext
                .Set<User>()
                .Where(user => ids.Contains(user.Id))
                .ToListAsync(cancellationToken);

    public async Task<User> GetByEmailAsync(Email email, CancellationToken cancellationToken = default) =>
        await _dbContext
            .Set<User>()
            .FirstOrDefaultAsync(user => user.Email == email, cancellationToken);

    public async Task<bool> IsEmailUniqueAsync(Email email, CancellationToken cancellationToken = default) =>
        !await _dbContext
            .Set<User>()
            .AnyAsync(user => user.Email == email, cancellationToken);

    public void Add(User user) =>
        _dbContext.Set<User>().Add(user);

    public void Update(User user) =>
        _dbContext.Set<User>().Update(user);

    public void Delete(User user) =>
        _dbContext.Set<User>().Remove(user);

    public async Task<User> GetUserByRoleAsync(Role role, CancellationToken cancellationToken)
    {
        return await _dbContext.Set<User>()
            .FirstOrDefaultAsync(user => user.Roles.Contains(role), cancellationToken);
    }
}
