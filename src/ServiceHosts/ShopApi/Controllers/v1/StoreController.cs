using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ShareMicroservice.Query.Api;
using Shop.Application.Dto.Request.Store;
using Shop.Application.Dto.Response.Store;
using Shop.Facade.Store;
using System.Net;

namespace ShopApi.Controllers.v1;

[ApiVersion("1")]
[Route("v{version:apiVersion}/[controller]/[Action]")]
[Authorize]
public class StoreController(
    IStoreFacade storeFacade,
    ILogger<StoreController> logger) : BaseController
{
    /// <summary>
    /// ایجاد فروشگاه
    /// </summary>
    /// <param name="request"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    [HttpPost, ProducesResponseType(typeof(ApiResult), (int)HttpStatusCode.OK)]
    public async Task<ApiResult> CreateStore(RequestCreateStoreDto request, CancellationToken cancellationToken)
    {
        try
        {
            var result = await storeFacade.CreateStore(new(
                request.UserIdentityId,
                request.Name,
                request.PhoneNumber,
                request.AvatarUrl),
                cancellationToken);

            return result.IsSuccess ? Ok() : BadRequest();
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error occurred while creating store: {Message}", ex.Message);
            return BadRequest();
        }
    }

    /// <summary>
    /// ویرایش فروشگاه با آیدی
    /// </summary>
    /// <param name="request"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    [HttpPost, ProducesResponseType(typeof(ApiResult), (int)HttpStatusCode.OK)]
    public async Task<ApiResult> UpdateStoreById(RequestUpdateStoreDto request, CancellationToken cancellationToken)
    {
        try
        {
            var result = await storeFacade.UpdateStoreById(new(
                request.Id,
                request.Name,
                request.PhoneNumber,
                request.AvatarUrl),
                cancellationToken);

            return result.IsSuccess ? Ok() : BadRequest();
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error occurred while updating store {Id}: {Message}", request.Id, ex.Message);
            return BadRequest();
        }
    }

    /// <summary>
    /// حذف فروشگاه با آیدی
    /// </summary>
    /// <param name="id"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    [HttpPost, ProducesResponseType(typeof(ApiResult), (int)HttpStatusCode.OK)]
    public async Task<ApiResult> DeleteStoreById(long id, CancellationToken cancellationToken)
    {
        try
        {
            var result = await storeFacade.DeleteStoreById(new(id), cancellationToken);

            return result.IsSuccess ? Ok() : BadRequest();
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error occurred while deleting store {Id}: {Message}", id, ex.Message);
            return BadRequest();
        }
    }

    /// <summary>
    /// جزئیات فروشگاه با آیدی
    /// </summary>
    /// <param name="id"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    [HttpPost, ProducesResponseType(typeof(ApiResult<GetStoreByIdDto>), (int)HttpStatusCode.OK)]
    public async Task<ApiResult<GetStoreByIdDto>> GetStoreById(long id, CancellationToken cancellationToken)
    {
        try
        {
            var result = await storeFacade.GetStoreById(new(id), cancellationToken);

            return result != null ? Ok(result) : BadRequest();
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error occurred while get store {Id}: {Message}", id, ex.Message);
            return BadRequest();
        }
    }
}
