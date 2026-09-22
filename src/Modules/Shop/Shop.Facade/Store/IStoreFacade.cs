using MediatR;
using ShareMicroservice.Application;
using Shop.Application.Command.Store;
using Shop.Application.Dto.Response.Store;
using Shop.Application.Query.Store;

namespace Shop.Facade.Store;

public interface IStoreFacade
{
    #region Command
    Task<ServiceResult> CreateStore(CreateStoreCommand request, CancellationToken cancellationToken);
    Task<ServiceResult> UpdateStoreById(UpdateStoreByIdCommand request, CancellationToken cancellationToken);
    Task<ServiceResult> DeleteStoreById(DeleteStoreByIdCommand request, CancellationToken cancellationToken);
    #endregion

    #region Query
    Task<GetStoreByIdDto> GetStoreById(RequestGetStoreById request, CancellationToken cancellationToken);
    #endregion
}

public class StoreFacade(ISender sender) : IStoreFacade
{
    #region Command
    public async Task<ServiceResult> CreateStore(CreateStoreCommand request, CancellationToken cancellationToken)
    => await sender.Send(request, cancellationToken);
    public async Task<ServiceResult> DeleteStoreById(DeleteStoreByIdCommand request, CancellationToken cancellationToken)
        => await sender.Send(request, cancellationToken);
    public async Task<ServiceResult> UpdateStoreById(UpdateStoreByIdCommand request, CancellationToken cancellationToken)
        => await sender.Send(request, cancellationToken);
    #endregion

    #region Query
    public async Task<GetStoreByIdDto> GetStoreById(RequestGetStoreById request, CancellationToken cancellationToken)
        => await sender.Send(request, cancellationToken);
    #endregion
}