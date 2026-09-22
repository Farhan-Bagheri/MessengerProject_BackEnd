using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using ShareMicroservice.Application.IBaseRequest;
using ShareMicroservice.Query.Extentions;
using Shop.Application.Dto.Response.Category;
using Shop.Infrastructure.Context;

namespace Shop.Application.Query.Category;

public record RequestGetCategoryById(long Id) : IBaseRequest<GetCategoryByIdDto>;
public class RequestGetCategoryByIdHandler(
    IShopContext context,
    ILogger<RequestGetCategoryByIdHandler> logger) : IBaseRequestHandler<RequestGetCategoryById, GetCategoryByIdDto>
{
    public async Task<GetCategoryByIdDto> Handle(RequestGetCategoryById request, CancellationToken cancellationToken)
    {
        try
        {
            var category = await context.Categories
                .Where(x => x.Id == request.Id)
                .Select(x => new GetCategoryByIdDto
                {
                    Id = x.Id,
                    Name = x.Name,
                    Slug = x.Slug,
                    StatusTypeId = (int)x.StatusType,
                    StatusTypeText = x.StatusType.ToDisplay(DisplayProperty.Name) ?? "",
                    ParentId = x.ParentId,
                    ParentName = x.Parent.Name ?? "",
                    CreatedAt = x.CreatedAt.ToPersianDate(),
                    UpdatedAt = x.UpdatedAt.ToPersianDate()
                }).FirstOrDefaultAsync(cancellationToken);

            return category;
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error occurred while get category id : {Id} : {Message}", request.Id, ex.Message);
            throw;
        }
    }
}
