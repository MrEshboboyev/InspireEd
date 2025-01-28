using InspireEd.Application.Abstractions.Messaging;
using InspireEd.Application.Classes.Queries.Common;
using InspireEd.Domain.Classes.Repositories;
using InspireEd.Domain.Shared;

namespace InspireEd.Application.Classes.Queries.GetAllClasses;

internal sealed class GetAllClassesQueryHandler(
    IClassRepository classRepository) : IQueryHandler<GetAllClassesQuery, List<ClassResponse>>
{
    public async Task<Result<List<ClassResponse>>> Handle(
        GetAllClassesQuery request,
        CancellationToken cancellationToken)
    {
        #region Get all Classes
        
        var classEntities = await classRepository.GetAllAsync(
            cancellationToken);
        
        #endregion
        
        #region Prepare response

        var classResponses = classEntities
            .Select(ClassResponseFactory.Create)
            .ToList();
        
        #endregion

        return Result.Success(classResponses);
    }
}