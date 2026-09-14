using Identity.Facade.User;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ShareMicroservice.Dto.Request.Identity;
using ShareMicroservice.Dto.Response.Identity;

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
                    ? SuccessResult(result.Data!.ToString())!
                    : BadRequestResult<string>(result.Message, result.Errorrs);
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
        public async Task<ApiResult<string>> LoginUserByUserNameAndPassword(
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
                    return BadRequestResult<string>("نام کاربری یا رمز عبور اشتباه است!");

                var jwtKey = configuration["Jwt:Key"];

                var jwtExpiryString = configuration["Jwt:ExpiryMinutes"];

                if (string.IsNullOrWhiteSpace(jwtKey))
                    return BadRequestResult<string>("تنظیمات JWT به درستی پیکربندی نشده است.");

                if (!int.TryParse(jwtExpiryString, out var jwtExpiry))
                    return BadRequestResult<string>("زمان انقضای JWT به درستی پیکربندی نشده است.");

                var token = await userFacade.GeneratedJwtTokenForUser(new(
                    userId.ToString(),
                    jwtKey,
                    jwtExpiry),
                    cancellationToken);

                if (String.IsNullOrWhiteSpace(token))
                    return BadRequestResult<string>("خطا در ایجاد توکن ورود.");

                return SuccessResult(token);
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
                var userId = await userFacade.GetUserIdExistByUserNameAndPassword(new(
                    request.UserName,
                    request.Password),
                    cancellationToken);


            }
            catch (Exception ex)
            {
                logger.LogError(ex.Message);
                throw;
            }
        }
    }
}