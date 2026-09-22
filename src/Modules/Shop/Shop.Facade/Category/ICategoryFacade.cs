using MediatR;
using ShareMicroservice.Application;
using Shop.Application.Command.Category;
using Shop.Application.Dto.Response.Category;
using Shop.Application.Query.Category;

namespace Shop.Facade.Category;

public interface ICategoryFacade
{
    #region Command
    Task<ServiceResult> CreateCategory(CreateCategoryCommand request, CancellationToken cancellationToken);
    Task<ServiceResult> UpdateCategoryById(UpdateCategoryByIdCommand request, CancellationToken cancellationToken);
    Task<ServiceResult> DeleteCategoryById(DeleteCategoryByIdCommand request, CancellationToken cancellationToken);
    #endregion

    #region Query
    Task<GetCategoryByIdDto> GetCategoryById(RequestGetCategoryById request, CancellationToken cancellationToken);
    Task<List<GetCategoryListByParentIdDto>> GetCategoryListByParentId(RequestGetCategoryListByParentId request, CancellationToken cancellationToken);
    #endregion
}
public class CategoryFacade(ISender sender) : ICategoryFacade
{
    #region Command
    public async Task<ServiceResult> CreateCategory(CreateCategoryCommand request, CancellationToken cancellationToken)
        => await sender.Send(request, cancellationToken);
    public async Task<ServiceResult> UpdateCategoryById(UpdateCategoryByIdCommand request, CancellationToken cancellationToken)
        => await sender.Send(request, cancellationToken);
    public async Task<ServiceResult> DeleteCategoryById(DeleteCategoryByIdCommand request, CancellationToken cancellationToken)
        => await sender.Send(request, cancellationToken);
    #endregion

    #region Query
    public async Task<GetCategoryByIdDto> GetCategoryById(RequestGetCategoryById request, CancellationToken cancellationToken)
        => await sender.Send(request, cancellationToken);
    public async Task<List<GetCategoryListByParentIdDto>> GetCategoryListByParentId(RequestGetCategoryListByParentId request, CancellationToken cancellationToken)
        => await sender.Send(request, cancellationToken);
    #endregion
}