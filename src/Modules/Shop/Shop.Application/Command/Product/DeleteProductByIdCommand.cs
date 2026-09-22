using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using ShareMicroservice.Application;
using ShareMicroservice.Application.IBaseRequest;
using Shop.Infrastructure.Context;

namespace Shop.Application.Command.Product;

public record DeleteProductByIdCommand(long Id) : IBaseRequest;
public class DeleteProductByIdCommandHandler(
    IShopContext context,
    ILogger<DeleteProductByIdCommandHandler> logger) : IBaseRequestHandler<DeleteProductByIdCommand>
{
    public async Task<ServiceResult> Handle(DeleteProductByIdCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var result = await context.Products
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
            logger.LogError(ex, "Error occurred while deleting product {Id}: {Message}", request.Id, ex.Message);
            throw;
        }
    }
}
