using System;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace VerticalViews.Request;

public delegate Task<IResult> RequestHandlerDelegate();

public interface IRequestBehavior<TRequest, TViewModel>
{
    Task<IResult> Handle(TRequest request, RequestHandlerDelegate next, CancellationToken cancellationToken);
}

public interface IRequestBehavior<TRequest>
    where TRequest : ViewRequest
{
    Task<IResult> Handle(TRequest request, RequestHandlerDelegate next, CancellationToken cancellationToken);
}