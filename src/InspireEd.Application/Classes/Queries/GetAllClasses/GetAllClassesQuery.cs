using InspireEd.Application.Abstractions.Messaging;
using InspireEd.Application.Classes.Queries.Common;

namespace InspireEd.Application.Classes.Queries.GetAllClasses;

public sealed record GetAllClassesQuery() : IQuery<List<ClassResponse>>;