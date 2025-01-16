using InspireEd.Application.Abstractions.Messaging;
using InspireEd.Application.Classes.Queries.Common;
using InspireEd.Domain.Classes.Repositories;
using InspireEd.Domain.Errors;
using InspireEd.Domain.Shared;

namespace InspireEd.Application.Classes.Queries.GetClassesBySubjectId;

internal sealed class GetClassesBySubjectIdQueryHandler(
    IClassRepository classRepository) : IQueryHandler<GetClassesBySubjectIdQuery, List<ClassResponse>>
{
    public async Task<Result<List<ClassResponse>>> Handle(
        GetClassesBySubjectIdQuery request,
        CancellationToken cancellationToken)
    {
        var subjectId = request.SubjectId;
        
        #region Get classes by this subject
        
        var classEntities = await classRepository.GetBySubjectIdAsync(
            subjectId, 
            cancellationToken);
        if (classEntities is null || classEntities.Count == 0)
        {
            return Result.Failure<List<ClassResponse>>(
                DomainErrors.Class.NotFoundBySubjectId(subjectId));
        }
        
        #endregion
        
        #region Prepare response

        var classResponses = classEntities
            .Select(ClassResponseFactory.Create)
            .ToList();
        
        #endregion

        return Result.Success(classResponses);
    }
}