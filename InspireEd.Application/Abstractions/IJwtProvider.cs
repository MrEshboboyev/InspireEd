using InspireEd.Domain.Entities;

namespace InspireEd.Application.Abstractions;

public interface IJwtProvider
{
    Task<string> GenerateAsync(User user);
}
