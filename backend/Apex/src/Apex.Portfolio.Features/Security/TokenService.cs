using System.Security.Cryptography;
using Microsoft.IdentityModel.Tokens;

namespace Apex.Portfolio.Features.Security;

public static class TokenService
{
    public static RsaSecurityKey CreateValidationKey(string publicKey)
    {
        using var rsa = RSA.Create();
        rsa.ImportFromPem(publicKey);

        var key = new RsaSecurityKey(rsa.ExportParameters(includePrivateParameters: false));
        if (key.KeySize < 2048)
            throw new InvalidOperationException("Authentication:PublicKey must be an RSA key of at least 2048 bits");

        return key;
    }
}
