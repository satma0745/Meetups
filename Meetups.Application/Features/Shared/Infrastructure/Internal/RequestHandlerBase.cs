namespace Meetups.Application.Features.Shared.Infrastructure.Internal;

using System.Threading.Tasks;

public abstract class RequestHandlerBase<TRequest, TResult, TErrorType>
    where TErrorType : System.Enum
{
    protected static Response<TResult, TErrorType> Success(TResult internalResult) =>
        new(internalResult);

    protected static Response<TResult, TErrorType> Failure(TErrorType errorType) =>
        new(errorType);

    public abstract Task<Response<TResult, TErrorType>> HandleRequest(TRequest request);
}