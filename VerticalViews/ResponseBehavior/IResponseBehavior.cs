using System;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace VerticalViews.ResponseBehavior;

public delegate Task<IResult> RequestHandlerDelegate();

public interface IResponseBehavior<TViewModel>
{
    Task<IResult> Handle(IViewRequest<TViewModel> request, RequestHandlerDelegate next, CancellationToken cancellationToken);
}

public abstract class ResponseBehavior<TViewModel>
{
    public abstract Task<IResult> Handle(ViewRequest request, RequestHandlerDelegate next, CancellationToken cancellationToken);
}

