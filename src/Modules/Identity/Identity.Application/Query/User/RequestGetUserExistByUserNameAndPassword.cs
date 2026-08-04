using MediatR;
using Microsoft.AspNetCore.Identity;

namespace Identity.Application.Query.User;

public record RequestGetUserExistByUserNameAndPassword(string UserName, string Password) : IRequest<bool>;

public class RequestGetUserExistByUserNameAndPasswordHandler(UserManager<Domain.Entities.User> userManager)
    : IRequestHandler<RequestGetUserExistByUserNameAndPassword, bool>
{
    public async Task<bool> Handle(RequestGetUserExistByUserNameAndPassword request,
        CancellationToken cancellationToken)
    {
        try
        {
            var findUser = await userManager.FindByNameAsync(request.UserName);
            if (findUser == null)
                return false;

            var checkPassword = await userManager.CheckPasswordAsync(findUser, request.Password);

            return checkPassword;
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex);
            throw;
        }
    }
}