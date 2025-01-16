using InspireEd.Application.Abstractions.Messaging;
using InspireEd.Application.Faculties.DepartmentHeads.Queries.Common;

namespace InspireEd.Application.Faculties.DepartmentHeads.Queries.GetDepartmentHeadDetails;

public sealed record GetDepartmentHeadDetailsQuery(
    Guid DepartmentHeadId) : IQuery<DepartmentHeadResponse>;