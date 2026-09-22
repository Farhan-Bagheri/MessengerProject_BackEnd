using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using ShareMicroservice.Application;
using ShareMicroservice.Application.IBaseRequest;
using Shop.Domain.Enums;
using Shop.Infrastructure.Context;

namespace Shop.Application.Command.Category;

public record UpdateCategoryByIdCommand(
    long Id,
    string Name,
    CategoryStatusType StatusType,
    long ParentId) : IBaseRequest;
public class UpdateCategoryByIdCommandHandler(
    IShopContext context,
    ILogger<UpdateCategoryByIdCommandHandler> logger) : IBaseRequestHandler<UpdateCategoryByIdCommand>
{
    public async Task<ServiceResult> Handle(UpdateCategoryByIdCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var result = await context.Categories
                .Where(x => x.Id == request.Id)
                .ExecuteUpdateAsync(x => x
                    .SetProperty(sp => sp.Name, request.Name)
                    .SetProperty(sp => sp.StatusType, request.StatusType)
                    .SetProperty(sp => sp.ParentId, request.ParentId)
                    .SetProperty(sp => sp.UpdatedAt, DateTime.Now));

            return result > 0
                ? ServiceResult.Success()
                : ServiceResult.Error();
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error occurred while updating category {Id}: {Message}", request.Id, ex.Message);
            throw;
        }
    }
}
