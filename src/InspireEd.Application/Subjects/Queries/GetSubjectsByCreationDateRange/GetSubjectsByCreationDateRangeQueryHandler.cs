using InspireEd.Application.Abstractions.Messaging;
using InspireEd.Application.Subjects.Queries.Common;
using InspireEd.Domain.Subjects.Repositories;
using InspireEd.Domain.Shared;

namespace InspireEd.Application.Subjects.Queries.GetSubjectsByCreationDateRange;

internal sealed class GetSubjectsByCreationDateRangeQueryHandler(
    ISubjectRepository subjectRepository) : IQueryHandler<GetSubjectsByCreationDateRangeQuery, List<SubjectResponse>>
{
    public async Task<Result<List<SubjectResponse>>> Handle(
        GetSubjectsByCreationDateRangeQuery request,
        CancellationToken cancellationToken)
    {
        var (startDate, endDate) = request;
        
        #region Get this Subjects
        
        var subjects = await subjectRepository.GetByCreationDateRangeAsync(
            startDate,
            endDate, cancellationToken);
        
        #endregion
        
        #region Prepare response
        
        var subjectResponses = subjects
            .Select(SubjectResponseFactory.Create)
            .ToList();
        
        #endregion

        return Result.Success(subjectResponses);
    }
}