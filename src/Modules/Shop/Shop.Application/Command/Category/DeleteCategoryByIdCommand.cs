using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using ShareMicroservice.Application;
using ShareMicroservice.Application.IBaseRequest;
using Shop.Infrastructure.Context;

namespace Shop.Application.Command.Category;

public record DeleteCategoryByIdCommand(long Id) : IBaseRequest;
public class DeleteCategoryByIdCommandHandler(
    IShopContext context,
    ILogger<DeleteCategoryByIdCommandHandler> logger) : IBaseRequestHandler<DeleteCategoryByIdCommand>
{
    public async Task<ServiceResult> Handle(DeleteCategoryByIdCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var result = await context.Categories
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
            logger.LogError(ex, "Error occurred while deleting category {Id}: {Message}", request.Id, ex.Message);
            throw;
        }
    }
}
