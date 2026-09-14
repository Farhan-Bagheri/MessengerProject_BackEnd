using Identity.Facade.User;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ShareMicroservice.Dto.Request.Identity;
using ShareMicroservice.Dto.Response.Identity;
using ShareMicroservice.Query.Api;
using ShareMicroservice.Query.Api.Jwt;

namespace IdentityApi.Controllers.v1.Account
{
    [ApiVersion("1")]
    [Route("v{version:apiVersion}/[controller]/[Action]")]
    [Authorize]
    public class AccountController(
        IUserFacade userFacade,
        ILogger<AccountController> logger,
        IConfiguration configuration) : BaseController
    {
        /// <summary>
        /// ثبت نام کاربر با نام کاربری
        /// </summary>
        /// <returns></returns>
        [HttpPost, AllowAnonymous]
        public async Task<ApiResult<string>> RegisterUserByUserNameAndPassword(
            RegisterUserByUserNameAndPasswordDto request,
            CancellationToken cancellationToken)
        {
            try
            {
                var result = await userFacade.CreateUser(
                    new(request.UserName, request.Password, request.ConfirmPassword),
                    cancellationToken);

                return result.IsSuccess
                    ? Ok(result.Data!.ToString())!
                    : BadRequest(result.Message);
            }
            catch (Exception ex)
            {
                logger.LogError(ex.Message);
                throw;
            }
        }

        /// <summary>
        /// ورود با نام کاربری و رمز عبور
        /// </summary>
        /// <returns></returns>
        [HttpPost, AllowAnonymous]
        public async Task<ApiResult<AccessTokenDto>> LoginUserByUserNameAndPassword(
            RequestLoginUserByUserNameAndPasswordDto request,
            CancellationToken cancellationToken)
        {
            try
            {
                var userId = await userFacade.GetUserIdExistByUserNameAndPassword(new(
                    request.UserName,
                    request.Password),
                    cancellationToken);

                if (userId == Guid.Empty)
                    return BadRequest("نام کاربری یا رمز عبور اشتباه است!");

                var jwtKey = configuration["Jwt:Key"];

                var jwtExpiryString = configuration["Jwt:ExpiryMinutes"];

                if (string.IsNullOrWhiteSpace(jwtKey))
                    return BadRequest("تنظیمات JWT به درستی پیکربندی نشده است.");

                if (!int.TryParse(jwtExpiryString, out var jwtExpiry))
                    return BadRequest("زمان انقضای JWT به درستی پیکربندی نشده است.");

                var token = await userFacade.GeneratedJwtTokenForUser(new(
                    userId.ToString(),
                    jwtKey,
                    jwtExpiry),
                    cancellationToken);

                return Ok(token);
            }
            catch (Exception ex)
            {
                logger.LogError(ex.Message);
                throw;
            }
        }

        /// <summary>
        /// ورود با نام کاربری و رمز عبور
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public async Task<ApiResult<GetUserByUserIdDto>> GetUserProfile(CancellationToken cancellationToken)
        {
            try
            {
                var result = await userFacade.GetUserByUserId(new(GetCurrentUserId), cancellationToken);

                return Ok(result);
            }
            catch (Exception ex)
            {
                logger.LogError(ex.Message);
                throw;
            }
        }
    }
}