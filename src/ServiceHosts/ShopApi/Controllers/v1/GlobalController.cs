using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ShareMicroservice.Query.Api;
using ShareMicroservice.Query.Dto;
using ShareMicroservice.Query.Extentions;
using Shop.Domain.Enums;
using System.Net;

namespace ShopApi.Controllers.v1;

[ApiVersion("1")]
[Route("v{version:apiVersion}/[controller]/[Action]")]
[Authorize]
public class GlobalController(
    ILogger<GlobalController> logger) : BaseController
{
    /// <summary>
    /// سلکت لیست وضعیت های دسته بندی
    /// </summary>
    /// <returns></returns>
    [HttpGet, AllowAnonymous, ProducesResponseType(typeof(ApiResult<List<SelectListItemDto>>), (int)HttpStatusCode.OK)]
    public async Task<ApiResult<List<SelectListItemDto>>> GetCategoryStatusTypeSelectList()
    {
        try
        {
            var result = EnumExtension.ToSelectListItemsDto<CategoryStatusType>();

            return result != null ? Ok(result) : BadRequest();
        }
        catch (Exception ex)
        {
            logger.LogError(ex, ex.Message);
            return BadRequest();
        }
    }
}
