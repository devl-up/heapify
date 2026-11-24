namespace Heapify.Application.Common.Requests;

public interface IRequestHandler<in TRequest>
{
    Task HandleAsync(TRequest request, CancellationToken cancellationToken);
}

public interface IRequestHandler<in TRequest, TResult>
{
    Task<TResult> HandleAsync(TRequest request, CancellationToken cancellationToken);
}