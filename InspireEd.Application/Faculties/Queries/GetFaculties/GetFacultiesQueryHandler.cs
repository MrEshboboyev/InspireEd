using InspireEd.Application.Abstractions.Messaging;
using InspireEd.Application.Faculties.Queries.Common;
using InspireEd.Domain.Faculties.Repositories;
using InspireEd.Domain.Shared;

namespace InspireEd.Application.Faculties.Queries.GetFaculties;

internal sealed class GetFacultiesQueryHandler(
    IFacultyRepository facultyRepository) : IQueryHandler<GetFacultiesQuery, FacultyListResponse>
{
    private readonly IFacultyRepository _facultyRepository = facultyRepository;
    
    public async Task<Result<FacultyListResponse>> Handle(
        GetFacultiesQuery request,
        CancellationToken cancellationToken)
    {
        #region Get All Faculties
        
        var faculties = await _facultyRepository.GetFacultiesAsync(
            cancellationToken);
        
        #endregion
        
        #region Prepare FacultyListResponse
        
        var response = new FacultyListResponse(
            faculties
                .Select(FacultyResponseFactory.Create)
                .ToList());
        
        #endregion
        
        return Result.Success(response);
    }
}