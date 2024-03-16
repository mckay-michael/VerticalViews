using MediatR;
using Microsoft.AspNetCore.Http;

namespace VerticalViews.Request;

public interface IRequestPipeline<TRequest, TViewModel, TMediatorReqeust>
    where TRequest : IViewRequest<TMediatorReqeust, TViewModel>
    where TMediatorReqeust : IRequest<TViewModel>
{
    Task<IResult> Handle(TRequest request, bool isPartailView, CancellationToken cancellationToken);
}

public interface IRequestPipeline<TRequest>
    where TRequest : ViewRequest
{
    Task<IResult> Handle(TRequest request, bool isPartailView, CancellationToken cancellationToken);
}