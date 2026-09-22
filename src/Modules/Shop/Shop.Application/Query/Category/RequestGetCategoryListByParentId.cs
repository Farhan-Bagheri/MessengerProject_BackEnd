using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using ShareMicroservice.Application.IBaseRequest;
using ShareMicroservice.Query.Extentions;
using Shop.Application.Dto.Response.Category;
using Shop.Domain.Enums;
using Shop.Infrastructure.Context;

namespace Shop.Application.Query.Category;

public record RequestGetCategoryListByParentId(long? ParentId, CategoryStatusType? StatusType) : IBaseRequest<List<GetCategoryListByParentIdDto>>;
public class RequestGetCategoryListByParentIdHandler(
    IShopContext context,
    ILogger<RequestGetCategoryListByParentIdHandler> logger) : IBaseRequestHandler<RequestGetCategoryListByParentId, List<GetCategoryListByParentIdDto>>
{
    public async Task<List<GetCategoryListByParentIdDto>> Handle(RequestGetCategoryListByParentId request, CancellationToken cancellationToken)
    {
        try
        {
            var result = await context.Categories
                .Where(x => (request.StatusType == null || x.StatusType == request.StatusType) &&
                            (request.ParentId == null || x.ParentId == request.ParentId))
                .Select(x => new GetCategoryListByParentIdDto
                {
                    Id = x.Id,
                    Name = x.Name,
                    StatusTypeId = (int)x.StatusType,
                    StatusTypeText = x.StatusType.ToDisplay(DisplayProperty.Name)
                }).ToListAsync(cancellationToken);

            return result;
        }
        catch (Exception ex)
        {
            logger.LogError(ex, ex.Message);
            throw;
        }
    }
}
