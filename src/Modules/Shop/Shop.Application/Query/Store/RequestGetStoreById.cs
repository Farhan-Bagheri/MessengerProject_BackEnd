using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using ShareMicroservice.Application.IBaseRequest;
using ShareMicroservice.Query.Extentions;
using Shop.Application.Dto.Response.Store;
using Shop.Infrastructure.Context;

namespace Shop.Application.Query.Store;

public record RequestGetStoreById(long Id) : IBaseRequest<GetStoreByIdDto>;
public class RequestGetStoreByIdHandler(
    IShopContext context,
    ILogger<RequestGetStoreByIdHandler> logger) : IBaseRequestHandler<RequestGetStoreById, GetStoreByIdDto>
{
    public async Task<GetStoreByIdDto> Handle(RequestGetStoreById request, CancellationToken cancellationToken)
    {
        try
        {
            var store = await context.Stores
                .Where(x => x.Id == request.Id)
                .Select(x => new GetStoreByIdDto()
                {
                    UserIdentityId = x.UserIdentityId,
                    Name = x.Name,
                    PhoneNumber = x.PhoneNumber,
                    UniqueCode = x.UniqueCode,
                    AvatarUrl = x.AvatarUrl,
                    CreatedAt = x.CreatedAt.ToPersianDate(),
                    UpdatedAt = x.UpdatedAt.ToPersianDate(),
                }).FirstOrDefaultAsync(cancellationToken);

            return store;
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error occurred while get store id : {Id} : {Message}", request.Id, ex.Message);
            throw;
        }
    }
}
