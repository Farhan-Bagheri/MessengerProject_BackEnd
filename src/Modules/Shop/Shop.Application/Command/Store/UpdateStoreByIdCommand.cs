using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using ShareMicroservice.Application;
using ShareMicroservice.Application.IBaseRequest;
using Shop.Infrastructure.Context;

namespace Shop.Application.Command.Store;

public record UpdateStoreByIdCommand(
    long Id,
    string Name,
    string PhoneNumber,
    string AvatarUrl) : IBaseRequest;
public class UpdateStoreByIdCommandHandler(
    IShopContext context,
    ILogger<UpdateStoreByIdCommandHandler> logger) : IBaseRequestHandler<UpdateStoreByIdCommand>
{
    public async Task<ServiceResult> Handle(UpdateStoreByIdCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var result = await context.Stores
                .Where(x => x.Id == request.Id)
                .ExecuteUpdateAsync(x => x
                    .SetProperty(sp => sp.Name, request.Name)
                    .SetProperty(sp => sp.PhoneNumber, request.PhoneNumber)
                    .SetProperty(sp => sp.AvatarUrl, request.AvatarUrl)
                    .SetProperty(sp => sp.UpdatedAt, DateTime.Now));

            return result > 0
                ? ServiceResult.Success()
                : ServiceResult.Error();
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error occurred while updating store {Id}: {Message}", request.Id, ex.Message);
            throw;
        }
    }
}
