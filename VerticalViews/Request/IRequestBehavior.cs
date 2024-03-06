using System;
using Microsoft.AspNetCore.Http;

namespace VerticalViews.Request;

public delegate Task<IResult> RequestHandlerDelegate();

public interface IRequestBehavior<TRequest, TViewModel>
    where TRequest : IViewRequest<TViewModel>
{
    Task<IResult> Handle(RequestHandlerDelegate next, CancellationToken cancellationToken);
}

public interface IRequestBehavior<TRequest>
    where TRequest : ViewRequest
{
    Task<IResult> Handle(RequestHandlerDelegate next, CancellationToken cancellationToken);
}