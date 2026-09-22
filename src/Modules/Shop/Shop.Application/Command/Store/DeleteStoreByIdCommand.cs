using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using ShareMicroservice.Application;
using ShareMicroservice.Application.IBaseRequest;
using Shop.Infrastructure.Context;

namespace Shop.Application.Command.Store;

public record DeleteStoreByIdCommand(long Id) : IBaseRequest;
public class DeleteStoreByIdCommandHandler(
    IShopContext context,
    ILogger<DeleteStoreByIdCommandHandler> logger) : IBaseRequestHandler<DeleteStoreByIdCommand>
{
    public async Task<ServiceResult> Handle(DeleteStoreByIdCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var result = await context.Stores
                .Where(x => x.Id == request.Id)
                .ExecuteUpdateAsync(x => x
                    .SetProperty(x => x.IsDelete, true),
                    cancellationToken);

            return result > 0
                ? ServiceResult.Success()
                : ServiceResult.Error();
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error occurred while deleting store {Id}: {Message}", request.Id, ex.Message);
            throw;
        }
    }
}
