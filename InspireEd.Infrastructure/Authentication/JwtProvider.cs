using InspireEd.Application.Abstractions.Security;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using InspireEd.Domain.Users.Entities;

namespace InspireEd.Infrastructure.Authentication;

/// <summary>
/// Provides functionality for generating JSON Web Tokens (JWT) for authenticated users.
/// </summary>
internal sealed class JwtProvider(
    IOptions<JwtOptions> options,
    IPermissionService permissionService) : IJwtProvider
{
    #region Private fields

    // Holds the JWT options configuration
    private readonly JwtOptions _options = options.Value;
    private readonly IPermissionService _permissionService = permissionService;

    #endregion

    /// <summary>
    /// Generates a JWT for the specified user.
    /// </summary>
    /// <param name="user">The user for whom the token is being generated.</param>
    /// <returns>A JWT as a string.</returns>
    public async Task<string> GenerateAsync(User user)
    {
        #region Create Claims List

        // Create a list of claims for the JWT
        var claims = new List<Claim>
        {
            new(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
            new(JwtRegisteredClaimNames.Email, user.Email.Value)
        };

        #endregion
        
        #region Permissions
        
        var permissions = await _permissionService
            .GetPermissionsAsync(user.Id);

        claims.AddRange(permissions.Select(permission => 
            new Claim(CustomClaims.Permissions, permission)));

        #endregion

        #region Create signing credentials

        // Create signing credentials using the secret key from options
        var signingCredentials = new SigningCredentials(
            new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_options.SecretKey)),
            SecurityAlgorithms.HmacSha256);

        #endregion

        #region New Jwt Security token

        // Create the JWT security token
        var token = new JwtSecurityToken(
            _options.Issuer,
            _options.Audience,
            claims,
            null,
            DateTime.UtcNow.AddHours(1),
            signingCredentials);

        #endregion

        #region Create Jwt Token

        // Write the token to a string
        var tokenValue = new JwtSecurityTokenHandler().WriteToken(token);

        #endregion

        return tokenValue;
    }
}