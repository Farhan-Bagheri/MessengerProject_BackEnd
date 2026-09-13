using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Identity.Application.Command.Jwt;

public record GeneratedJwtTokenForUserCommand(
    string UserId,
    string JwtKey,
    int JwtExpiry)
    : IRequest<string>;

public class GeneratedJwtTokenForUserCommandHandler(
    UserManager<Domain.Entities.User> userManager)
    : IRequestHandler<GeneratedJwtTokenForUserCommand, string>
{
    public async Task<string> Handle(
        GeneratedJwtTokenForUserCommand request,
        CancellationToken cancellationToken)
    {
        try
        {
            var user = await userManager.FindByIdAsync(request.UserId);

            if (user == null)
                return String.Empty;

            var claims = new[]
            {
                new Claim("Id", user.Id.ToString()),
                new Claim("Email", user.Email ?? string.Empty),
                new Claim("UserName", user.UserName ?? string.Empty),
                new Claim("Jti", Guid.NewGuid().ToString())
            };

            var key = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(request.JwtKey));

            var credentials = new SigningCredentials(
                key,
                SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(request.JwtExpiry),
                signingCredentials: credentials);

            var tokenString = new JwtSecurityTokenHandler().WriteToken(token);

            return tokenString;
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex);
            throw;
        }
    }
}