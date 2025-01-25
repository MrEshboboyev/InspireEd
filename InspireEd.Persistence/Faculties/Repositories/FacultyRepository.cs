using InspireEd.Domain.Faculties.Entities;
using InspireEd.Domain.Faculties.Repositories;
using Microsoft.EntityFrameworkCore;

namespace InspireEd.Persistence.Faculties.Repositories;

public sealed class FacultyRepository(ApplicationDbContext dbContext) : IFacultyRepository
{
    public async Task<IEnumerable<Faculty>> GetFacultiesAsync(
        CancellationToken cancellationToken = default)
        => await dbContext
            .Set<Faculty>()
            .AsNoTracking()
            .Include(f => f.Groups)
            .ToListAsync(cancellationToken);

    public async Task<Faculty> GetByIdAsync(
        Guid id,
        CancellationToken cancellationToken = default)
        => await dbContext
            .Set<Faculty>()
            .AsNoTracking()
            .Include(f => f.Groups)
            .FirstOrDefaultAsync(f => f.Id == id, cancellationToken);

    public void Add(Faculty faculty)
    => dbContext.Set<Faculty>().Add(faculty);

    public void Update(Faculty faculty)
    => dbContext.Set<Faculty>().Update(faculty);

    public void Remove(Faculty faculty) 
    => dbContext.Set<Faculty>().Remove(faculty);
}