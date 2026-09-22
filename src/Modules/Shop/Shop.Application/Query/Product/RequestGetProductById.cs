using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using ShareMicroservice.Application.IBaseRequest;
using ShareMicroservice.Query.Extentions;
using Shop.Application.Dto.Response.Product;
using Shop.Infrastructure.Context;

namespace Shop.Application.Query.Product;

public record RequestGetProductById(long Id) : IBaseRequest<GetProductByIdDto>;
public class RequestGetProductByIdHandler(
    IShopContext context,
    ILogger<RequestGetProductByIdHandler> logger) : IBaseRequestHandler<RequestGetProductById, GetProductByIdDto>
{
    public async Task<GetProductByIdDto> Handle(RequestGetProductById request, CancellationToken cancellationToken)
    {
        try
        {
            var product = await context.Products
                .Where(x => x.Id == request.Id)
                .Select(x => new GetProductByIdDto
                {
                    Id = x.Id,
                    Title = x.Title,
                    Description = x.Description,
                    Images = x.Images,
                    CreatedAt = x.CreatedAt.ToPersianDate(),
                    UpdatedAt = x.UpdatedAt.ToPersianDate()
                }).FirstOrDefaultAsync(cancellationToken);

            return product;
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error occurred while get product id : {Id} : {Message}", request.Id, ex.Message);
            throw;
        }
    }
}
