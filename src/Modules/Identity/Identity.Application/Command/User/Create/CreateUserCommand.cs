using MediatR;
using Microsoft.AspNetCore.Identity;
using ShareMicroservice.Application;

namespace Identity.Application.Command.User.Create;

public record CreateUserCommand(string UserName, string Password, string ConfirmPassword)
    : IRequest<ServiceResult>;

public class CreateUserCommandHandler(UserManager<Domain.Entities.User> userManager)
    : IRequestHandler<CreateUserCommand, ServiceResult>
{
    public async Task<ServiceResult> Handle(CreateUserCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var existingUser = await userManager.FindByNameAsync(request.UserName);
            if (existingUser != null)
                return ServiceResult.Error($"User {existingUser.UserName} already exists.");

            if (request.Password != request.ConfirmPassword)
                return ServiceResult.Error($"Passwords do not match.");

            var user = new Domain.Entities.User()
            {
                UserName = request.UserName,
                IsActive = true,
                CreatedAt = DateTime.Now,
                UpdatedAt = DateTime.Now,
            };

            var result = await userManager.CreateAsync(user, request.Password);

            List<string> errors = result.Errors.Select(error => error.Description).ToList();

            return result.Succeeded ? ServiceResult.Success(user.UserName) : ServiceResult.Error("Error", errors: errors);
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex);
            throw;
        }
    }
}