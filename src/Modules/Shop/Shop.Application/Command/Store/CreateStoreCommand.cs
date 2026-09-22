using Microsoft.Extensions.Logging;
using ShareMicroservice.Application;
using ShareMicroservice.Application.IBaseRequest;
using Shop.Infrastructure.Context;

namespace Shop.Application.Command.Store;

public record CreateStoreCommand(
    string UserIdentityId,
    string Name,
    string PhoneNumber,
    string AvatarUrl) : IBaseRequest;
public class CreateStoreCommandHandler(
    IShopContext context,
    ILogger<CreateStoreCommandHandler> logger) : IBaseRequestHandler<CreateStoreCommand>
{
    public async Task<ServiceResult> Handle(CreateStoreCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var store = new Domain.Entities.Store(
                request.UserIdentityId,
                request.Name,
                request.PhoneNumber,
                request.AvatarUrl);

            await context.Stores.AddAsync(store, cancellationToken);

            var result = await context.SaveChangesAsync(cancellationToken);

            return result > 0
                ? ServiceResult.Success()
                : ServiceResult.Error();
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error occurred while creating store: {Message}", ex.Message);
            throw;
        }
    }
}
