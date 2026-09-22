using Microsoft.Extensions.Logging;
using ShareMicroservice.Application;
using ShareMicroservice.Application.IBaseRequest;
using Shop.Domain.Class;
using Shop.Infrastructure.Context;

namespace Shop.Application.Command.Product;

public record CreateProductCommand(
    string Title,
    string Description,
    List<ProductImageUrl> Images) : IBaseRequest;
public class CreateProductCommandHandler(
    IShopContext context,
    ILogger<CreateProductCommandHandler> logger) : IBaseRequestHandler<CreateProductCommand>
{
    public async Task<ServiceResult> Handle(CreateProductCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var product = new Domain.Entities.Product(
                request.Title,
                request.Description,
                request.Images);

            await context.Products.AddAsync(product, cancellationToken);

            var result = await context.SaveChangesAsync(cancellationToken);

            return result > 0
                ? ServiceResult.Success()
                : ServiceResult.Error();
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error occurred while creating product: {Message}", ex.Message);
            throw;
        }
    }
}
