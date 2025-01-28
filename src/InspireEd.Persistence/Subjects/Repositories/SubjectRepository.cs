using InspireEd.Domain.Subjects.Entities;
using InspireEd.Domain.Subjects.Repositories;
using InspireEd.Domain.Subjects.ValueObjects;
using Microsoft.EntityFrameworkCore;

namespace InspireEd.Persistence.Subjects.Repositories;

/// <summary>
/// Implements the repository interface for the Subject aggregate root.
/// </summary>
internal sealed class SubjectRepository(ApplicationDbContext dbContext) : ISubjectRepository
{
    public async Task<List<Subject>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return await dbContext.Set<Subject>().ToListAsync(cancellationToken);
    }
    
    public async Task<List<Subject>> GetByCreditRangeAsync(
        int minCredit, 
        int maxCredit,
        CancellationToken cancellationToken = default)
    {
        return await dbContext.Set<Subject>()
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
        return await dbContext.Set<Subject>()
            .Where(subject => 
                subject.CreatedOnUtc >= startDate &&
                subject.CreatedOnUtc <= endDate)
            .ToListAsync(cancellationToken);
    }
    
    public async Task<Subject> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await dbContext.Set<Subject>().FindAsync([id], cancellationToken);
    }
    
    public async Task<bool> IsNameUniqueAsync(SubjectName subjectName, CancellationToken cancellationToken = default) =>
        !await dbContext
            .Set<Subject>()
            .AnyAsync(subject => subject.Name == subjectName, cancellationToken);
    
    public async Task<bool> IsCodeUniqueAsync(SubjectCode subjectCode, CancellationToken cancellationToken = default) =>
        !await dbContext
            .Set<Subject>()
            .AnyAsync(subject => subject.Code == subjectCode, cancellationToken);

    public void Add(Subject subject)
    {
        dbContext.Set<Subject>().Add(subject);
    }

    public void Update(Subject subject)
    {
        dbContext.Set<Subject>().Update(subject);
    }

    public void Remove(Subject subject)
    {
        dbContext.Set<Subject>().Remove(subject);
    }
}