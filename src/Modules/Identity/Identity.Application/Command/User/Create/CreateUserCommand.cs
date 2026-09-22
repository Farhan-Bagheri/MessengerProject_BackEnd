using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using ShareMicroservice.Application;
using ShareMicroservice.Application.IBaseRequest;

namespace Identity.Application.Command.User.Create;

public record CreateUserCommand(string UserName, string Password, string ConfirmPassword)
    : IBaseRequest;

public class CreateUserCommandHandler(
    UserManager<Domain.Entities.User> userManager,
    ILogger<CreateUserCommandHandler> logger)
    : IBaseRequestHandler<CreateUserCommand>
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
            logger.LogError(ex, "Error occurred while creating user: {Message}", ex.Message);
            throw;
        }
    }
}