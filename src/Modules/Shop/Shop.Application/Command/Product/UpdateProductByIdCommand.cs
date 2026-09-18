using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using ShareMicroservice.Application;
using ShareMicroservice.Application.BaseCommand;
using ShareMicroservice.Domain.Class;
using Shop.Infrastructure.Context;

namespace Shop.Application.Command.Product;

public record UpdateProductByIdCommand(
    long Id,
    string Title,
    string Description,
    List<string> Images) : IBaseRequest;
public class UpdateProductByIdCommandHandler(
    IShopContext context,
    ILogger<UpdateProductByIdCommandHandler> logger) : IBaseRequestHandler<UpdateProductByIdCommand>
{
    public async Task<ServiceResult> Handle(UpdateProductByIdCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var images = request.Images?
                .Select(i => new ImageUrl() { Url = i })
                .ToList() ?? [];

            var result = await context.Products
                .Where(x => x.Id == request.Id)
                .ExecuteUpdateAsync(x => x
                    .SetProperty(sp => sp.Title, request.Title)
                    .SetProperty(sp => sp.Description, request.Description)
                    .SetProperty(sp => sp.Images, images)
                    .SetProperty(sp => sp.UpdatedAt, DateTime.Now));

            return result > 0 ? ServiceResult.Success() : ServiceResult.Error();
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error occurred while updating product {Id}: {Message}", request.Id, ex.Message);
            throw;
        }
    }
}
