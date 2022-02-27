namespace Meetups.Application.Features.Shared.Infrastructure.Api;

using System;
using System.Threading.Tasks;
using Meetups.Application.Features.Shared.Infrastructure.Internal;
using Microsoft.AspNetCore.Mvc;

public static class ApiPipeline
{
    public static TRequest CreateRequest<TRequest>(TRequest request) =>
        request;

    public static Task<Response<TResult, TErrorTypes>> HandleRequestAsync<TRequest, TResult, TErrorTypes>(
        this TRequest request,
        RequestHandlerBase<TRequest, TResult, TErrorTypes> requestHandler)
        where TErrorTypes : Enum =>
        requestHandler.HandleRequest(request);

    public static async Task<IActionResult> ToApiResponse<TResult, TErrorTypes>(
        this Task<Response<TResult, TErrorTypes>> internalResponseTask,
        Func<TResult, IActionResult> onSuccess,
        Func<TErrorTypes, IActionResult> onFailure)
        where TErrorTypes : Enum
    {
        var internalResponse = await internalResponseTask;
        return internalResponse.Success
            ? onSuccess(internalResponse.Payload)
            : onFailure(internalResponse.ErrorType);
    }
}