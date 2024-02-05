using System;
using Microsoft.AspNetCore.Http;

namespace VerticalViews.Request;

public delegate Task<IResult> RequestHandlerDelegate();

public interface IRequestBehavior<TRequest, TViewModel>
    where TRequest : BaseRequest
{
    Task<IResult> Handle(IViewRequest<TViewModel> request, RequestHandlerDelegate next, CancellationToken cancellationToken);
}