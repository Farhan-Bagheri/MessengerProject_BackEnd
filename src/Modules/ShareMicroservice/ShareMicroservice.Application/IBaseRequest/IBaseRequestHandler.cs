using MediatR;

namespace ShareMicroservice.Application.BaseCommand;

public interface IBaseRequestHandler<TRequest>
    : IRequestHandler<TRequest, ServiceResult>
    where TRequest : IBaseRequest
{
}

public interface IBaseRequestHandler<TRequest, TResponse>
    : IRequestHandler<TRequest, TResponse>
    where TRequest : IBaseRequest<TResponse>
{
}