namespace Apex.Users.Features.Security;

public class AuthOptions
{
    public string Issuer { get; set; } = null!;
    public string Audience { get; set; } = null!;
    public string Key { get; set; } = null!;
    public int AccessTokenInMinutes { get; set; } = 30;
    public int RefreshTokenInMinutes { get; set; } = 43200;
}
