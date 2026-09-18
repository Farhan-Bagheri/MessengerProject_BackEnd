using Microsoft.AspNetCore.Mvc;

namespace ShopApi.Controllers;

[ApiController]
public class BaseController : ControllerBase
{
    public string? GetCurrentUserId
    {
        get
        {
            try
            {
                if (User is { Identity.IsAuthenticated: true })
                {
                    string? userId = User.Claims.FirstOrDefault(x => x.Type == "Id")?.Value;
                    return string.IsNullOrWhiteSpace(userId) ? null : userId;
                }

                return null;
            }
            catch (NullReferenceException)
            {
                return null;
            }
        }
    }
}