using Microsoft.AspNetCore.Mvc;

namespace GatewayApi.Controllers;

[Route("api/[controller]")]
[ApiController]
public class GatewayController : ControllerBase
{
    /// <summary>
    /// تست اجرای گیت‌وی
    /// </summary>
    /// <returns></returns>
    [HttpGet]
    public IActionResult Get()
    {
        return Ok(new { Message = "Gateway is running!" });
    }
}
