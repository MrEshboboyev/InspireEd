using InspireEd.Application.Abstractions.Messaging;
using InspireEd.Application.Subjects.Queries.Common;
using InspireEd.Domain.Subjects.Repositories;
using InspireEd.Domain.Shared;

namespace InspireEd.Application.Subjects.Queries.GetAllSubjects;

internal sealed class GetAllSubjectsQueryHandler(
    ISubjectRepository subjectRepository) : IQueryHandler<GetAllSubjectsQuery, List<SubjectResponse>>
{
    public async Task<Result<List<SubjectResponse>>> Handle(
        GetAllSubjectsQuery request,
        CancellationToken cancellationToken)
    {
        var subjects = await subjectRepository.GetAllAsync(cancellationToken);
        
        var subjectResponses = subjects
            .Select(SubjectResponseFactory.Create)
            .ToList();

        return Result.Success(subjectResponses);
    }
}