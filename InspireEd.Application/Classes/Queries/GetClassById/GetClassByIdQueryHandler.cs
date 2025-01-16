using InspireEd.Application.Abstractions.Messaging;
using InspireEd.Application.Classes.Queries.Common;
using InspireEd.Domain.Classes.Repositories;
using InspireEd.Domain.Errors;
using InspireEd.Domain.Shared;

namespace InspireEd.Application.Classes.Queries.GetClassById;

internal sealed class GetClassByIdQueryHandler(
    IClassRepository classRepository) : IQueryHandler<GetClassByIdQuery, ClassResponse>
{
    public async Task<Result<ClassResponse>> Handle(
        GetClassByIdQuery request,
        CancellationToken cancellationToken)
    {
        var classId = request.Id;
        
        #region Get this Class
        
        var classEntity = await classRepository.GetByIdAsync(
            classId,
            cancellationToken);
        
        #endregion
        
        return classEntity == null 
            ? Result.Failure<ClassResponse>(
                DomainErrors.Class.NotFound(request.Id)) 
            : Result.Success(ClassResponseFactory.Create(classEntity));
    }
}