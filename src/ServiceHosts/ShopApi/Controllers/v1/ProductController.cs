using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ShareMicroservice.Query.Api;
using Shop.Application.Dto.Request.Product;
using Shop.Facade.Product;
using System.Net;

namespace ShopApi.Controllers.v1;

[ApiVersion("1")]
[Route("v{version:apiVersion}/[controller]/[Action]")]
[Authorize]
public class ProductController(
    IProductFacade productFacade,
    ILogger<ProductController> logger) : BaseController
{
    /// <summary>
    /// ایجاد محصول
    /// </summary>
    /// <param name="request"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    [HttpPost, ProducesResponseType(typeof(ApiResult), (int)HttpStatusCode.OK)]
    public async Task<ApiResult> CreateProduct(RequestCreateProductDto request, CancellationToken cancellationToken)
    {
        try
        {
            var result = await productFacade.CreateProduct(new(
                request.Title,
                request.Description,
                request.Images),
                cancellationToken);

            return result.IsSuccess ? Ok() : BadRequest();
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error occurred while creating product: {Message}", ex.Message);
            return BadRequest();
        }
    }

    /// <summary>
    /// ویرایش محصول با آیدی
    /// </summary>
    /// <param name="request"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    [HttpPost, ProducesResponseType(typeof(ApiResult), (int)HttpStatusCode.OK)]
    public async Task<ApiResult> UpdateProductById(RequestUpdateProductByIdDto request, CancellationToken cancellationToken)
    {
        try
        {
            var result = await productFacade.UpdateProductById(new(
                request.Id,
                request.Title,
                request.Description,
                request.Images),
                cancellationToken);

            return result.IsSuccess ? Ok() : BadRequest();
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error occurred while updating product {Id}: {Message}", request.Id, ex.Message);
            return BadRequest();
        }
    }
}
