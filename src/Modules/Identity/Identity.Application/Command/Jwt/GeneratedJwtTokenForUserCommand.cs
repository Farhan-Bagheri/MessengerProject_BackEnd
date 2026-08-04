using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using MediatR;
using Microsoft.IdentityModel.Tokens;
using ShareMicroservice.Application.Common;

namespace Identity.Application.Command.Jwt;

public record GeneratedJwtTokenForUserCommand(Domain.Entities.User User, string JwtKey, int JwtExpiry)
    : IRequest<ServiceResult>;

public class GeneratedJwtTokenForUserCommandHandler : IRequestHandler<GeneratedJwtTokenForUserCommand, ServiceResult>
{
    public async Task<ServiceResult> Handle(GeneratedJwtTokenForUserCommand request,
        CancellationToken cancellationToken)
    {
        try
        {
            {
                var claims = new[]
                {
                    new Claim("Id", request.User.Id.ToString()),
                    new Claim("Email", request.User.Email!),
                    new Claim("UserName", request.User.UserName!),
                    new Claim("Jti", Guid.NewGuid().ToString())
                };

                var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(request.JwtKey));
                var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

                var token = new JwtSecurityToken(
                    //issuer: _jwtIssuer,
                    //audience: _jwtAudience,
                    claims: claims,
                    expires: DateTime.Now.AddDays(request.JwtExpiry),
                    signingCredentials: creds);

                return ServiceResult.Success(token.ToString());
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex);
            throw;
        }
    }
}