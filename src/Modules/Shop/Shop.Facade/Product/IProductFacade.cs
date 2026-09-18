using MediatR;
using ShareMicroservice.Application;
using Shop.Application.Command.Product;

namespace Shop.Facade.Product;

public interface IProductFacade
{
    #region Command
    Task<ServiceResult> CreateProduct(CreateProductCommand request, CancellationToken cancellationToken);
    Task<ServiceResult> UpdateProductById(UpdateProductByIdCommand request, CancellationToken cancellationToken);
    #endregion
}
public class ProductFacade(ISender sender) : IProductFacade
{
    #region Command
    public async Task<ServiceResult> CreateProduct(CreateProductCommand request, CancellationToken cancellationToken)
        => await sender.Send(request, cancellationToken);
    public async Task<ServiceResult> UpdateProductById(UpdateProductByIdCommand request, CancellationToken cancellationToken)
        => await sender.Send(request, cancellationToken);
    #endregion
}