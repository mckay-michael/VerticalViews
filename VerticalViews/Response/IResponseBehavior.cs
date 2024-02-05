using System;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace VerticalViews.ResponseBehavior;

public delegate Task<IResult> RequestHandlerDelegate();

public interface IResponseBehavior<TRequest, TViewModel>
    where TRequest : BaseRequest
{
    Task<IResult> Handle(TViewModel response, RequestHandlerDelegate next, CancellationToken cancellationToken);
}