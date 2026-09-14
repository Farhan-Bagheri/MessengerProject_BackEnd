using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using ShareMicroservice.Dto.Response.Identity;

namespace Identity.Application.Query.User;

public record RequestGetUserByUserId(string UserId) : IRequest<GetUserByUserIdDto>;
public class RequestGetUserByUserIdHandler(
    UserManager<Domain.Entities.User> userManager,
    ILogger<RequestGetUserByUserIdHandler> logger) : IRequestHandler<RequestGetUserByUserId, GetUserByUserIdDto>
{
    public async Task<GetUserByUserIdDto> Handle(RequestGetUserByUserId request, CancellationToken cancellationToken)
    {
        try
        {
            var user = await userManager.FindByIdAsync(request.UserId);
            if (user == null) return new();

            var result = new GetUserByUserIdDto()
            {
                Id = user.Id.ToString(),
                UserName = user.UserName,
                Email = user.Email,
                PhoneNumber = user.PhoneNumber,
                FirstName = user.FirstName,
                LastName = user.LastName,
                BirthDate = user.BirthDate.ToString(),
                CreatedAt = user.CreatedAt.ToString(),
                UpdatedAt = user.UpdatedAt.ToString(),
                IsActive = user.IsActive
            };

            return result;
        }
        catch (Exception ex)
        {
            logger.LogError(ex, ex.Message);
            throw;
        }
    }
}
