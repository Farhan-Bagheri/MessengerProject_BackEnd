using Microsoft.Extensions.Logging;
using ShareMicroservice.Application;
using ShareMicroservice.Application.IBaseRequest;
using Shop.Domain.Enums;
using Shop.Infrastructure.Context;

namespace Shop.Application.Command.Category;

public record CreateCategoryCommand(
    string Name,
    string Slug,
    CategoryStatusType StatusType,
    long ParentId) : IBaseRequest;
public class CreateCategoryCommandHandler(
    IShopContext context,
    ILogger<CreateCategoryCommandHandler> logger) : IBaseRequestHandler<CreateCategoryCommand>
{
    public async Task<ServiceResult> Handle(CreateCategoryCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var category = new Domain.Entities.Category(
                request.Name,
                request.Slug,
                request.StatusType,
                request.ParentId);

            await context.Categories.AddAsync(category, cancellationToken);

            var result = await context.SaveChangesAsync(cancellationToken);

            return result > 0
                ? ServiceResult.Success()
                : ServiceResult.Error();
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error occurred while creating category: {Message}", ex.Message);
            throw;
        }
    }
}
