using Identity.Facade.User;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ShareMicroservice.Common.Class.ApiResult;
using ShareMicroservice.Dto.Request.Identity;

namespace IdentityApi.Controllers.v1.Account
{
    [ApiVersion("1")]
    [Route("v{version:apiVersion}/[controller]/[Action]")]
    [Authorize]
    public class AccountController(IUserFacade userFacade) : BaseController
    {
        /// <summary>
        /// Register User By UserName And Password
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
                Console.WriteLine(ex);
                throw;
            }
        }

        /// <summary>
        /// Login By UserName Password
        /// </summary>
        /// <returns></returns>
        [HttpPost, AllowAnonymous]
        public async Task<ApiResult> LoginUserByUserNameAndPassword()
        {
            try
            {
                return ApiResult.SuccessResult();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return BadRequestResult();
            }
        }
    }
}