using MediatR;
using ShareMicroservice.Application;
using Shop.Application.Command.Product;
using Shop.Application.Dto.Response.Product;
using Shop.Application.Query.Product;

namespace Shop.Facade.Product;

public interface IProductFacade
{
    #region Command
    Task<ServiceResult> CreateProduct(CreateProductCommand request, CancellationToken cancellationToken);
    Task<ServiceResult> UpdateProductById(UpdateProductByIdCommand request, CancellationToken cancellationToken);
    Task<ServiceResult> DeleteProductById(DeleteProductByIdCommand request, CancellationToken cancellationToken);
    #endregion

    #region Query
    Task<GetProductByIdDto> GetProductById(RequestGetProductById request, CancellationToken cancellationToken);
    #endregion
}
public class ProductFacade(ISender sender) : IProductFacade
{
    #region Command
    public async Task<ServiceResult> CreateProduct(CreateProductCommand request, CancellationToken cancellationToken)
        => await sender.Send(request, cancellationToken);
    public async Task<ServiceResult> UpdateProductById(UpdateProductByIdCommand request, CancellationToken cancellationToken)
        => await sender.Send(request, cancellationToken);
    public async Task<ServiceResult> DeleteProductById(DeleteProductByIdCommand request, CancellationToken cancellationToken)
        => await sender.Send(request, cancellationToken);
    #endregion

    #region Query
    public async Task<GetProductByIdDto> GetProductById(RequestGetProductById request, CancellationToken cancellationToken)
        => await sender.Send(request, cancellationToken);
    #endregion
}