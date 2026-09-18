using MediatR;

namespace ShareMicroservice.Application.BaseCommand;

public interface IBaseRequest : IRequest<ServiceResult>
{
}

public interface IBaseRequest<TResponse> : IRequest<TResponse>
{
}