using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using ShareMicroservice.Query.Api.Jwt;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Identity.Application.Command.Jwt;

public record GeneratedJwtTokenForUserCommand(
    string UserId,
    string JwtKey,
    string JwtIssuer,
    string JwtAudience,
    int JwtExpiry)
    : IRequest<AccessTokenDto>;

public class GeneratedJwtTokenForUserCommandHandler(
    UserManager<Domain.Entities.User> userManager)
    : IRequestHandler<GeneratedJwtTokenForUserCommand, AccessTokenDto>
{
    public async Task<AccessTokenDto> Handle(
        GeneratedJwtTokenForUserCommand request,
        CancellationToken cancellationToken)
    {
        var user = await userManager.FindByIdAsync(request.UserId);

        if (user == null)
            return null;

        var refreshToken = Guid.NewGuid().ToString("N");
        var refreshTokenSerial = Guid.NewGuid().ToString();

        var expireDate = DateTime.UtcNow.AddMinutes(request.JwtExpiry);

        var claims = new[]
        {
            new Claim("Id", user.Id.ToString()),
            new Claim("Email", user.Email ?? string.Empty),
            new Claim("UserName", user.UserName ?? string.Empty),
            new Claim("Jti", Guid.NewGuid().ToString())
        };

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(request.JwtKey));

        var credentials = new SigningCredentials(
            key,
            SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            issuer: request.JwtIssuer,
            audience: request.JwtAudience,
            claims: claims,
            expires: expireDate,
            signingCredentials: credentials);

        var accessToken = new JwtSecurityTokenHandler()
            .WriteToken(token);

        return new AccessTokenDto
        {
            AccessToken = accessToken,
            RefreshToken = refreshToken,
            RefreshTokenSerial = refreshTokenSerial,
            ExpireDate = expireDate
        };
    }
}