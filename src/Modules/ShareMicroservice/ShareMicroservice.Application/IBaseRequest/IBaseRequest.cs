using MediatR;

namespace ShareMicroservice.Application.IBaseRequest;

public interface IBaseRequest : IRequest<ServiceResult>
{
}

public interface IBaseRequest<TResponse> : IRequest<TResponse>
{
}