using Apex.Users.Domain.Entities;

namespace Apex.Users.Features.Security;

public interface ITokenService
{
    string CreateAccessToken(User user, IReadOnlyCollection<string> roles);
    string GenerateRefreshToken();
    string HashToken(string token);
}
