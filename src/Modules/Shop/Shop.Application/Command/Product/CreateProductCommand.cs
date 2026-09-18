using Microsoft.Extensions.Logging;
using ShareMicroservice.Application;
using ShareMicroservice.Application.BaseCommand;
using ShareMicroservice.Domain.Class;
using Shop.Infrastructure.Context;

namespace Shop.Application.Command.Product;



public record CreateProductCommand(
    string Title,
    string Description,
    List<string> Images) : IBaseRequest;
public class CreateProductCommandHandker(
    IShopContext context,
    ILogger<CreateProductCommandHandker> logger) : IBaseRequestHandler<CreateProductCommand>
{
    public async Task<ServiceResult> Handle(CreateProductCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var product = new Domain.Entities.Product
            {
                Title = request.Title,
                Description = request.Description,
                Images = request.Images
                    .Select(x => new ImageUrl { Url = x })
                    .ToList()
            };

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
