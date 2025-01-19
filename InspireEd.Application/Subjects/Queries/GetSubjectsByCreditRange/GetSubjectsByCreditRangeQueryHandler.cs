using InspireEd.Application.Abstractions.Messaging;
using InspireEd.Application.Subjects.Queries.Common;
using InspireEd.Domain.Subjects.Repositories;
using InspireEd.Domain.Shared;

namespace InspireEd.Application.Subjects.Queries.GetSubjectsByCreditRange;

internal sealed class GetSubjectsByCreditRangeQueryHandler(
    ISubjectRepository subjectRepository) : IQueryHandler<GetSubjectsByCreditRangeQuery, List<SubjectResponse>>
{
    public async Task<Result<List<SubjectResponse>>> Handle(
        GetSubjectsByCreditRangeQuery request,
        CancellationToken cancellationToken)
    {
        var (minCredit, maxCredit) = request;
        
        #region Get this subjects
        
        var subjects = await subjectRepository.GetByCreditRangeAsync(
            minCredit, 
            maxCredit,
            cancellationToken);
        
        #endregion
        
        #region Prepare response
        
        var subjectResponses = subjects
            .Select(SubjectResponseFactory.Create)
            .ToList();
        
        #endregion

        return Result.Success(subjectResponses);
    }
}