using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using ShareMicroservice.Dto.Response.Identity.User;

namespace Identity.Application.Query.User;

public record RequestGetUserProfileByUserIdentityId(string UserIdentityId)
    : IRequest<GetUserProfileByUserIdentityIdDto>;
public class RequestGetUserProfileByUserIdentityIdHandler(
    UserManager<Domain.Entities.User> userManager,
    ILogger<RequestGetUserProfileByUserIdentityIdHandler> logger)
    : IRequestHandler<RequestGetUserProfileByUserIdentityId, GetUserProfileByUserIdentityIdDto>
{
    public async Task<GetUserProfileByUserIdentityIdDto> Handle(RequestGetUserProfileByUserIdentityId request, CancellationToken cancellationToken)
    {
        try
        {
            var user = await userManager.FindByIdAsync(request.UserIdentityId);

            var result = new GetUserProfileByUserIdentityIdDto()
            {
                UserIdentityId = user.Id.ToString(),
                FirstName = user.FirstName,
                LastName = user.LastName,
                PhoneNumber = user.PhoneNumber,
                UserName = user.UserName,
                AvatrUrl = user.UserAvatars.ToString()
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
