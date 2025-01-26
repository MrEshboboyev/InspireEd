using InspireEd.Application.Abstractions.Messaging;
using InspireEd.Application.Subjects.Queries.Common;
using InspireEd.Domain.Subjects.Repositories;
using InspireEd.Domain.Errors;
using InspireEd.Domain.Shared;

namespace InspireEd.Application.Subjects.Queries.GetSubjectById;

internal sealed class GetSubjectByIdQueryHandler(
    ISubjectRepository subjectRepository) : IQueryHandler<GetSubjectByIdQuery, SubjectResponse>
{
    public async Task<Result<SubjectResponse>> Handle(
        GetSubjectByIdQuery request,
        CancellationToken cancellationToken)
    {
        var subjectId = request.SubjectId;
        
        #region Get this Subject
        
        var subject = await subjectRepository.GetByIdAsync(
            subjectId,
            cancellationToken);
        if (subject is null)
        {
            return Result.Failure<SubjectResponse>(
                DomainErrors.Subject.NotFound(subjectId));
        }
        
        #endregion
        
        #region Prepare response

        var subjectResponse = SubjectResponseFactory.Create(subject);
        
        #endregion

        return Result.Success(subjectResponse);
    }
}