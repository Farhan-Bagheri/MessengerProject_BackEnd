using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ShareMicroservice.Query.Api;
using Shop.Application.Dto.Request.Category;
using Shop.Application.Dto.Response.Category;
using Shop.Domain.Enums;
using Shop.Facade.Category;
using System.Net;

namespace ShopApi.Controllers.v1;

[ApiVersion("1")]
[Route("v{version:apiVersion}/[controller]/[Action]")]
[Authorize]
public class CategoryFacade(
    ICategoryFacade categoryFacade,
    ILogger<CategoryFacade> logger) : BaseController
{
    /// <summary>
    /// ایجاد دسته بندی
    /// </summary>
    /// <param name="request"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    [HttpPost, ProducesResponseType(typeof(ApiResult), (int)HttpStatusCode.OK)]
    public async Task<ApiResult> CreateCategory(RequestCreateCategoryDto request, CancellationToken cancellationToken)
    {
        try
        {
            var result = await categoryFacade.CreateCategory(new(
                request.Name,
                request.Slug,
                (CategoryStatusType)request.StatusTypeId,
                request.ParentId),
                cancellationToken);

            return result.IsSuccess ? Ok() : BadRequest();
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error occurred while creating category: {Message}", ex.Message);
            return BadRequest();
        }
    }

    /// <summary>
    /// ویرایش دسته بندی با آیدی
    /// </summary>
    /// <param name="request"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    [HttpPost, ProducesResponseType(typeof(ApiResult), (int)HttpStatusCode.OK)]
    public async Task<ApiResult> UpdateCategoryById(RequestUpdateCategoryByIdDto request, CancellationToken cancellationToken)
    {
        try
        {
            var result = await categoryFacade.UpdateCategoryById(new(
                request.Id,
                request.Name,
                (CategoryStatusType)request.StatusTypeId,
                request.ParentId),
                cancellationToken);

            return result.IsSuccess ? Ok() : BadRequest();
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error occurred while updating category {Id}: {Message}", request.Id, ex.Message);
            return BadRequest();
        }
    }

    /// <summary>
    /// حذف دسته بندی با آیدی
    /// </summary>
    /// <param name="id"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    [HttpPost, ProducesResponseType(typeof(ApiResult), (int)HttpStatusCode.OK)]
    public async Task<ApiResult> DeleteCategoryById(long id, CancellationToken cancellationToken)
    {
        try
        {
            var result = await categoryFacade.DeleteCategoryById(new(id), cancellationToken);

            return result.IsSuccess ? Ok() : BadRequest();
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error occurred while deleting category {Id}: {Message}", id, ex.Message);
            return BadRequest();
        }
    }

    /// <summary>
    /// جزئیات دسته بندی با آیدی
    /// </summary>
    /// <param name="id"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    [HttpPost, ProducesResponseType(typeof(ApiResult<GetCategoryByIdDto>), (int)HttpStatusCode.OK)]
    public async Task<ApiResult<GetCategoryByIdDto>> GetCategoryById(long id, CancellationToken cancellationToken)
    {
        try
        {
            var result = await categoryFacade.GetCategoryById(new(id), cancellationToken);

            return result != null ? Ok(result) : BadRequest();
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error occurred while get category {Id}: {Message}", id, ex.Message);
            return BadRequest();
        }
    }

    /// <summary>
    /// لیست دسته بندی ها با دریافت آیدی والد
    /// </summary>
    /// <param name="parentId"></param>
    /// <param name="statusTypeId"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    [HttpPost, AllowAnonymous, ProducesResponseType(typeof(ApiResult<List<GetCategoryListByParentIdDto>>), (int)HttpStatusCode.OK)]
    public async Task<ApiResult<List<GetCategoryListByParentIdDto>>> GetCategoryListByParentId(long? parentId, int? statusTypeId, CancellationToken cancellationToken)
    {
        try
        {
            var result = await categoryFacade.GetCategoryListByParentId(new(
                parentId,
                (CategoryStatusType)statusTypeId),
                cancellationToken);

            return result != null ? Ok(result) : BadRequest();
        }
        catch (Exception ex)
        {
            logger.LogError(ex, ex.Message);
            return BadRequest();
        }
    }
}
