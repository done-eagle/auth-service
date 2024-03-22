using System.Security.Cryptography;
using System.Text;

namespace AuthService.Tests.Data;

internal static class PkceGen
{
    internal static string GenerateCodeVerifier()
    {
        const int codeVerifierLength = 64;
        var randomBytes = new byte[codeVerifierLength];
        using (var rng = RandomNumberGenerator.Create())
        {
            rng.GetBytes(randomBytes);
        }
        
        return Base64UrlEncode(randomBytes);
    }

    internal static string GenerateCodeChallenge(string codeVerifier)
    {
        using var sha256 = SHA256.Create();
        var challengeBytes = sha256.ComputeHash(Encoding.ASCII.GetBytes(codeVerifier));
        return Base64UrlEncode(challengeBytes);
    }

    private static string Base64UrlEncode(byte[] bytes)
    {
        var base64 = Convert.ToBase64String(bytes);
        return base64.Replace('+', '-').Replace('/', '_').TrimEnd('=');
    }
}