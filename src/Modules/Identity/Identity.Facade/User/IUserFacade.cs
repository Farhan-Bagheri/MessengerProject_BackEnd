using Identity.Application.Command.Jwt;
using Identity.Application.Command.User.Create;
using Identity.Application.Query.User;
using MediatR;
using ShareMicroservice.Application.Common;
using ShareMicroservice.Common.Api.Jwt;

namespace Identity.Facade.User;

public interface IUserFacade
{
    #region Command

    Task<ServiceResult> CreateUser(CreateUserCommand request, CancellationToken cancellationToken);

    Task<AccessTokenDto> GeneratedJwtTokenForUser(GeneratedJwtTokenForUserCommand request, CancellationToken cancellationToken);

    #endregion

    #region Query

    Task<Guid> GetUserIdExistByUserNameAndPassword(RequestGetUserIdByUserNameAndPassword request, CancellationToken cancellationToken);

    #endregion
}

public class UserFacade(ISender sender) : IUserFacade
{
    #region Command

    public async Task<ServiceResult> CreateUser(CreateUserCommand request, CancellationToken cancellationToken)
        => await sender.Send(request, cancellationToken);

    public async Task<AccessTokenDto> GeneratedJwtTokenForUser(GeneratedJwtTokenForUserCommand request, CancellationToken cancellationToken)
        => await sender.Send(request, cancellationToken);

    #endregion

    #region Query

    public async Task<Guid> GetUserIdExistByUserNameAndPassword(RequestGetUserIdByUserNameAndPassword request, CancellationToken cancellationToken)
        => await sender.Send(request, cancellationToken);

    #endregion
}