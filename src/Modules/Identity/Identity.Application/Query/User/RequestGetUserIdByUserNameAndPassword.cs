using MediatR;
using Microsoft.AspNetCore.Identity;

namespace Identity.Application.Query.User;

public record RequestGetUserIdByUserNameAndPassword(string UserName, string Password) : IRequest<Guid>;

public class RequestGetUserIdByUserNameAndPasswordHandler(UserManager<Domain.Entities.User> userManager)
    : IRequestHandler<RequestGetUserIdByUserNameAndPassword, Guid>
{
    public async Task<Guid> Handle(RequestGetUserIdByUserNameAndPassword request,
        CancellationToken cancellationToken)
    {
        try
        {
            var findUser = await userManager.FindByNameAsync(request.UserName);
            if (findUser == null) return Guid.Empty;

            var checkPassword = await userManager.CheckPasswordAsync(findUser, request.Password);
            if (!checkPassword) return Guid.Empty;

            return findUser.Id;
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex);
            throw;
        }
    }
}