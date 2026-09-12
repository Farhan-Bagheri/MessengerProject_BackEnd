using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ShareMicroservice.Common;

namespace ShopApi.Controllers.v1;

[ApiVersion("1")]
[Route("v{version:apiVersion}/[controller]/[Action]")]
[Authorize]
public class ProductController : BaseController
{
}
