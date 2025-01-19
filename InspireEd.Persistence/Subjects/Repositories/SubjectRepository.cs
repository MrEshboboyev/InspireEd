using InspireEd.Domain.Repositories;
using InspireEd.Domain.Subjects.Entities;
using InspireEd.Domain.Subjects.Repositories;
using Microsoft.EntityFrameworkCore;

namespace InspireEd.Persistence.Subjects.Repositories;

/// <summary>
/// Implements the repository interface for the Subject aggregate root.
/// </summary>
internal sealed class SubjectRepository(DbContext context) : ISubjectRepository
{
    public async Task<List<Subject>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return await context.Set<Subject>().ToListAsync(cancellationToken);
    }
    
    public async Task<List<Subject>> GetByCreditRangeAsync(
        int minCredit, 
        int maxCredit,
        CancellationToken cancellationToken = default)
    {
        return await context.Set<Subject>()
            .Where(subject => 
                subject.Credit.Value >= minCredit &&
                subject.Credit.Value <= maxCredit)
            .ToListAsync(cancellationToken);
    }
    
    public async Task<List<Subject>> GetByCreationDateRangeAsync(
        DateTime startDate, 
        DateTime endDate,
        CancellationToken cancellationToken = default)
    {
        return await context.Set<Subject>()
            .Where(subject => 
                subject.CreatedOnUtc >= startDate &&
                subject.CreatedOnUtc <= endDate)
            .ToListAsync(cancellationToken);
    }
    
    public async Task<Subject> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await context.Set<Subject>().FindAsync([id], cancellationToken);
    }

    public void Add(Subject subject)
    {
        context.Set<Subject>().Add(subject);
    }

    public void Update(Subject subject)
    {
        context.Set<Subject>().Update(subject);
    }

    public void Remove(Subject subject)
    {
        context.Set<Subject>().Remove(subject);
    }
}