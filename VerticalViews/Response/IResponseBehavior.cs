using System;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace VerticalViews.ResponseBehavior;

public delegate Task<IResult> ResponseHandlerDelegate();

public interface IResponseBehavior<TRequest, TViewModel>
    where TRequest : IBaseRequest
{
    Task<IResult> Handle(TViewModel response, ResponseHandlerDelegate next, CancellationToken cancellationToken);
}