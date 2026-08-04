using System.Threading;
using System.Threading.Tasks;
using Identity.Application.Command.Jwt;
using Identity.Application.Command.User.Create;
using Identity.Application.Query.User;
using MediatR;
using ShareMicroservice.Application.Common;

namespace Identity.Facade.User;

public interface IUserFacade
{
    #region Command

    Task<ServiceResult> CreateUser(CreateUserCommand request, CancellationToken cancellationToken);

    Task<ServiceResult> GeneratedJwtTokenForUser(GeneratedJwtTokenForUserCommand request,
        CancellationToken cancellationToken);

    #endregion

    #region Query

    Task<bool> GetUserExistByUserNameAndPassword(RequestGetUserExistByUserNameAndPassword request,
        CancellationToken cancellationToken);

    #endregion
}

public class UserFacade(ISender sender) : IUserFacade
{
    #region Command

    public async Task<ServiceResult> CreateUser(CreateUserCommand request, CancellationToken cancellationToken)
        => await sender.Send(request, cancellationToken);

    public async Task<ServiceResult> GeneratedJwtTokenForUser(GeneratedJwtTokenForUserCommand request,
        CancellationToken cancellationToken)
        => await sender.Send(request, cancellationToken);

    #endregion

    #region Query

    public async Task<bool> GetUserExistByUserNameAndPassword(RequestGetUserExistByUserNameAndPassword request,
        CancellationToken cancellationToken)
        => await sender.Send(request, cancellationToken);

    #endregion
}