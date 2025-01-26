using InspireEd.Application.Abstractions.Messaging;
using InspireEd.Application.Faculties.Queries.Common;
using InspireEd.Domain.Faculties.Repositories;
using InspireEd.Domain.Errors;
using InspireEd.Domain.Shared;

namespace InspireEd.Application.Faculties.Queries.GetFacultyDetails;

internal sealed class GetFacultyDetailsQueryHandler(
    IFacultyRepository facultyRepository) : IQueryHandler<GetFacultyDetailsQuery, FacultyResponse>
{
    public async Task<Result<FacultyResponse>> Handle(
        GetFacultyDetailsQuery request,
        CancellationToken cancellationToken)
    {
        var facultyId = request.FacultyId;
        
        #region Get this Faculty 
        
        var faculty = await facultyRepository.GetByIdAsync(
            facultyId,
            cancellationToken);
        if (faculty == null)
        {
            return Result.Failure<FacultyResponse>(
                DomainErrors.Faculty.NotFound(request.FacultyId));
        }
        
        #endregion
        
        #region Prepare response

        var facultyResponse = FacultyResponseFactory.Create(faculty);
        
        #endregion

        return Result.Success(facultyResponse);
    }
}