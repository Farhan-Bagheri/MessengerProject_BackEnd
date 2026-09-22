using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using ShareMicroservice.Application;
using ShareMicroservice.Application.IBaseRequest;
using Shop.Domain.Class;
using Shop.Infrastructure.Context;

namespace Shop.Application.Command.Product;

public record UpdateProductByIdCommand(
    long Id,
    string Title,
    string Description,
    List<ProductImageUrl> Images) : IBaseRequest;
public class UpdateProductByIdCommandHandler(
    IShopContext context,
    ILogger<UpdateProductByIdCommandHandler> logger) : IBaseRequestHandler<UpdateProductByIdCommand>
{
    public async Task<ServiceResult> Handle(UpdateProductByIdCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var result = await context.Products
                .Where(x => x.Id == request.Id)
                .ExecuteUpdateAsync(x => x
                    .SetProperty(sp => sp.Title, request.Title)
                    .SetProperty(sp => sp.Description, request.Description)
                    .SetProperty(sp => sp.Images, request.Images)
                    .SetProperty(sp => sp.UpdatedAt, DateTime.Now));

            return result > 0
                ? ServiceResult.Success()
                : ServiceResult.Error();
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error occurred while updating product {Id}: {Message}", request.Id, ex.Message);
            throw;
        }
    }
}
