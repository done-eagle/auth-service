using System.Security.Claims;
using AuthService.Api.Data;
using Microsoft.AspNetCore.Authentication;
using Newtonsoft.Json;

namespace AuthService.Api.Сonverters;

public class KcRoleConverter : IClaimsTransformation
{
    public Task<ClaimsPrincipal> TransformAsync(ClaimsPrincipal principal)
    {
        var identity = (ClaimsIdentity)principal.Identity!;

        if (identity.IsAuthenticated)
        {
            // Извлекаем роли из токена и добавляем их к Claims
            var claims = identity.Claims.ToList();

            var realmAccessClaims = claims.Where(claim => claim.Type == "realm_access")
                .Select(claim => JsonConvert.DeserializeObject<RolesData>(claim.Value));

            foreach (var data in realmAccessClaims)
            {
                foreach (var role in data!.Roles)
                {
                    identity.AddClaim(new Claim(ClaimTypes.Role, role));
                }
            }
        }

        return Task.FromResult(principal);
    }
}